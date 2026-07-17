using System.Net.Mail;
using System.Security.Claims;
using HelpDesk.Domain.Entities;
using HelpDesk.Repositories.Interfaces;
using HelpDesk.Services.Constants;
using HelpDesk.Services.DTOs.Common;
using HelpDesk.Services.DTOs.ForgotPasswordDTOs;
using HelpDesk.Services.DTOs.LoginDTOs;
using HelpDesk.Services.Enums;
using HelpDesk.Services.Interfaces;
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

    public AuthService(IUserRepository repository, IJwtService jwtService, AzureTokenValidator azureTokenValidator,
     IEmailService emailService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
        var role = (RoleEnum)user.RoleId;

        var response = new LoginResponse
        {
            AccessToken = token,
            RefreshToken = "",
            UserName = user.Name,
            Role = role.ToString(),
            Expiration = DateTime.UtcNow.AddHours(1)
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

        // var resetToken = new PasswordResetToken
        // {
        //     UserId = user.Id,
        //     Token = token,
        //     ExpiryDate = DateTime.UtcNow.AddMinutes(30),
        //     CreatedAt = DateTime.UtcNow,
        //     IsUsed = false
        // };

        // await _passwordResetRepository.Add(resetToken);

        // var resetLink = $"https://localhost:5173/reset-password?token={Uri.EscapeDataString(token)}";

        // await _emailService.SendEmailAsync(user.Email, "Reset Password", resetLink);

        return ResponseFactory.Success<object>(null);
    }

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