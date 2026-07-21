using System.Net.Mail;
using System.Security.Claims;
using HelpDesk.Domain.Entities;
using HelpDesk.Repositories.Interfaces;
using HelpDesk.Services.Constants;
using HelpDesk.Services.DTOs.Common;
using HelpDesk.Services.DTOs.ForgotPasswordDTOs;
using HelpDesk.Services.DTOs.LoginDTOs;
using HelpDesk.Services.DTOs.ProfileDTOs;
using HelpDesk.Services.Enums;
using HelpDesk.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace HelpDesk.Services.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IJwtService _jwtService;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly AzureTokenValidator _azureTokenValidator;
    private readonly IEmailService _emailService;
    private readonly HashSet<string> _internalDomains;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public AuthService(IUserRepository repository, IJwtService jwtService, AzureTokenValidator azureTokenValidator,
     IEmailService emailService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment)
    {
        _passwordHasher = new PasswordHasher<User>();
        _repository = repository;
        _jwtService = jwtService;
        _azureTokenValidator = azureTokenValidator;
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
        _internalDomains = configuration
            .GetSection("Authentication:InternalDomains")
            .Get<string[]>()?
            .ToHashSet(StringComparer.OrdinalIgnoreCase)
            ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        _configuration = configuration;
        _environment = environment;
    }

    private ClaimsPrincipal User => _httpContextAccessor.HttpContext!.User;

    public async Task<BaseResponse<LoginResponse>> Login(LoginRequest request, bool isSSO = false)
    {
        var user = await _repository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            return ResponseFactory.Failure<LoginResponse>(
                Messages.Auth.InvalidCredentials,
                StatusCodes.Status401Unauthorized
            );
        }

        if (!user.IsActive)
        {
            return ResponseFactory.Failure<LoginResponse>(
                Messages.Auth.AccountInactive,
                StatusCodes.Status403Forbidden
            );
        }

        if (user.IsDeleted)
        {
            return ResponseFactory.Failure<LoginResponse>(
                Messages.Auth.AccountDeleted,
                StatusCodes.Status403Forbidden
            );
        }

        if (!isSSO)
        {
            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return ResponseFactory.Failure<LoginResponse>(
                    Messages.Auth.InvalidCredentials,
                    StatusCodes.Status401Unauthorized
                );
            }
        }

        var token = _jwtService.GenerateJwtToken(user);
        var refreshToken = _jwtService.GenerateToken();
        RefreshToken newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.UserId,
            Token = refreshToken,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpiryInDays"]))
        };

        await _repository.AddRefreshToken(newRefreshToken);

        var role = (RoleEnum)user.RoleId;

        var response = new LoginResponse
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            UserName = user.Name ?? "",
            Role = role.ToString(),
        };

        return ResponseFactory.Success(
            response,
            Messages.Auth.LoginSuccess,
            StatusCodes.Status200OK
        );
    }

    public async Task<BaseResponse<LoginResponse>> SSOLogin(string token)
    {
        ClaimsPrincipal principal = await _azureTokenValidator.ValidateAsync(token);

        var email = principal.FindFirst("preferred_username")?.Value;

        if (string.IsNullOrWhiteSpace(email))
        {
            return ResponseFactory.Failure<LoginResponse>(
                Messages.Auth.SSOEmailRetrievalFailed,
                StatusCodes.Status401Unauthorized
            );
        }

        if (!IsInternalEmail(email))
        {
            return ResponseFactory.Failure<LoginResponse>(
                Messages.Auth.InternalUsersOnly,
                StatusCodes.Status401Unauthorized
            );
        }

        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = string.Empty
        };

        return await Login(loginRequest, true);
    }

    public async Task<BaseResponse<LoginResponse>> RefreshTokenAsync(string token)
    {
        var refreshToken = await _repository.GetByTokenAsync(token);

        if (refreshToken == null || DateTime.UtcNow >= refreshToken.ExpiresAt)
            return ResponseFactory.Failure<LoginResponse>(
                Messages.Auth.InvalidToken,
                StatusCodes.Status401Unauthorized
            );

        User user = refreshToken.User;
        if (!user.IsActive)
        {
            return ResponseFactory.Failure<LoginResponse>(
                Messages.Auth.AccountInactive,
                StatusCodes.Status403Forbidden
            );
        }

        if (user.IsDeleted)
        {
            return ResponseFactory.Failure<LoginResponse>(
                Messages.Auth.AccountDeleted,
                StatusCodes.Status403Forbidden
            );
        }

        var accessToken = _jwtService.GenerateJwtToken(user);

        var role = (RoleEnum)user.RoleId;
        LoginResponse response = new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = token,
            UserName = user.Name ?? "",
            Role = role.ToString(),
        };

        return ResponseFactory.Success(
            response,
            Messages.Auth.LoginSuccess,
            StatusCodes.Status200OK
        );
    }

    public async Task<BaseResponse<object>> ForgotPassword(ForgotPasswordRequest request)
    {
        var user = await _repository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            return ResponseFactory.Failure<object>(
                Messages.Auth.UserNotExists,
                StatusCodes.Status401Unauthorized
            );
        }

        if (!user.IsActive)
        {
            return ResponseFactory.Failure<object>(
                Messages.Auth.AccountInactive,
                StatusCodes.Status403Forbidden
            );
        }

        if (user.IsDeleted)
        {
            return ResponseFactory.Failure<object>(
                Messages.Auth.AccountDeleted,
                StatusCodes.Status403Forbidden
            );
        }

        var token = _jwtService.GenerateToken();
        ResetPasswordToken resetToken = new ResetPasswordToken
        {
            Id = Guid.NewGuid(),
            UserId = user.UserId,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["PasswordReset:TokenExpirationMinutes"])),
            CreatedAt = DateTime.UtcNow,
            IsUsed = false
        };

        await _repository.AddResetPasswordToken(resetToken);

        var resetLink = $"{_configuration["Frontend:BaseUrl"]}/reset-password?token={Uri.EscapeDataString(token)}";

        string subject = "Reset Your Help Desk Password";

        var path = Path.Combine(_environment.ContentRootPath, "EmailTemplates", "ResetPassword.html");

        var html = await File.ReadAllTextAsync(path);

        html = html.Replace("{{Name}}", user.Name);
        html = html.Replace("{{ResetLink}}", resetLink);

        await _emailService.SendEmailAsync(user.Email, subject, html);

        return ResponseFactory.Success<object>(
            new object(),
            token,
            StatusCodes.Status200OK);
    }

    public async Task<BaseResponse<object>> ResetPassword(ResetPasswordTokenRequest request)
    {
        var token = await _repository.GetByResetPasswordTokenAsync(request.Token);

        if (token == null || DateTime.UtcNow >= token.ExpiresAt || token.IsUsed)
            return ResponseFactory.Failure<object>(
                Messages.Auth.InvalidToken,
                StatusCodes.Status401Unauthorized
            );

        User user = token.User;
        if (!user.IsActive)
        {
            return ResponseFactory.Failure<object>(
                Messages.Auth.AccountInactive,
                StatusCodes.Status403Forbidden
            );
        }

        if (user.IsDeleted)
        {
            return ResponseFactory.Failure<object>(
                Messages.Auth.AccountDeleted,
                StatusCodes.Status403Forbidden
            );
        }

        var verifyResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash!,
            request.NewPassword);


        token.IsUsed = true;
        await _repository.UpdateResetPasswordToken(token);

        if (verifyResult == PasswordVerificationResult.Success)
        {
            return ResponseFactory.Success<object>(
                new object(),
                Messages.Profile.PasswordChangeSuccess,
                StatusCodes.Status200OK);
        }

        user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
        user.UpdatedOn = DateTime.UtcNow;

        await _repository.UpdateUser(user);

        return ResponseFactory.Success<object>(
            new object(),
            Messages.Profile.PasswordChangeSuccess,
            StatusCodes.Status200OK);
    }

    public async Task<BaseResponse<ProfileResponse>> GetProfile()
    {
        User? user = await _repository.GetUserById(UserId);

        if (user == null)
        {
            return ResponseFactory.Failure<ProfileResponse>(
                Messages.General.NotFound,
                StatusCodes.Status404NotFound);
        }

        return ResponseFactory.Success(
            MapToResponse(user),
            Messages.General.Success,
            StatusCodes.Status200OK);
    }

    public async Task<BaseResponse<ProfileResponse>> UpdateProfile(UpdateProfileRequest request)
    {
        User? user = await _repository.GetUserById(UserId);

        if (user == null)
        {
            return ResponseFactory.Failure<ProfileResponse>(
                Messages.General.NotFound,
                StatusCodes.Status404NotFound);
        }

        // Update name directly
        user.Name = request.Name.Trim();
        user.UpdatedOn = DateTime.UtcNow;

        await _repository.UpdateUser(user);

        return ResponseFactory.Success(
            MapToResponse(user),
            Messages.Profile.UpdateSuccess,
            StatusCodes.Status200OK);
    }

    public async Task<BaseResponse<object>> ChangePassword(ChangePasswordRequest request)
    {
        User? user = await _repository.GetUserById(UserId);

        if (user == null)
        {
            return ResponseFactory.Failure<object>(
                Messages.General.NotFound,
                StatusCodes.Status404NotFound);
        }

        var verifyResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash!,
            request.CurrentPassword);

        if (verifyResult == PasswordVerificationResult.Failed)
        {
            return ResponseFactory.Failure<object>(
                Messages.Profile.CurrentPasswordInvalid,
                StatusCodes.Status400BadRequest);
        }

        user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
        user.UpdatedOn = DateTime.UtcNow;

        await _repository.UpdateUser(user);

        return ResponseFactory.Success<object>(
            new object(),
            Messages.Profile.PasswordChangeSuccess,
            StatusCodes.Status200OK);
    }

    private static ProfileResponse MapToResponse(User user) => new()
    {
        UserId = user.UserId,
        Name = user.Name,
        Email = user.Email,
        Role = user.Role?.RoleName ?? string.Empty,
        IsActive = user.IsActive
    };

    public string Email => User.FindFirstValue(ClaimTypes.Email)!;

    public RoleEnum Role => Enum.Parse<RoleEnum>(User.FindFirstValue(ClaimTypes.Role)!);

    public Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    public bool IsInternalEmail(string email)
    {
        try
        {
            var mail = new MailAddress(email);

            return _internalDomains.Contains(mail.Host.ToLower());
        }
        catch
        {
            return false;
        }
    }
}