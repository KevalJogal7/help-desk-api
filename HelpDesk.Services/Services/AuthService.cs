using System.Security.Claims;
using HelpDesk.Domain.Entities;
using HelpDesk.Repositories.Interfaces;
using HelpDesk.Services.DTOs;
using HelpDesk.Services.DTOs.Common;
using HelpDesk.Services.Enums;
using HelpDesk.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace HelpDesk.Services.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IJwtService _jwtService;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly AzureTokenValidator _azureTokenValidator;

    public AuthService(IUserRepository repository, IJwtService jwtService, AzureTokenValidator azureTokenValidator)
    {
        _passwordHasher = new PasswordHasher<User>();
        _repository = repository;
        _jwtService = jwtService;
        _azureTokenValidator = azureTokenValidator;
    }


    public async Task<BaseResponse<LoginResponse>> Login(LoginRequest request, bool isSSO = false)
    {
        var user = await _repository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            return ResponseFactory.Failure<LoginResponse>(
                "Invalid email or password",
                StatusCodes.Status401Unauthorized
            );
        }

        if (!user.IsActive)
        {
            return ResponseFactory.Failure<LoginResponse>(
                "Your account is inactive.",
                StatusCodes.Status403Forbidden
            );
        }

        if (user.IsDeleted)
        {
            return ResponseFactory.Failure<LoginResponse>(
                "User account has been deleted.",
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
                    "Invalid email or password",
                    StatusCodes.Status401Unauthorized
                );
            }
        }

        var token = _jwtService.GenerateToken(user);

        var response = new LoginResponse
        {
            AccessToken = token,
            RefreshToken = "",
            UserName = $"{user.FirstName} {user.LastName}",
            RoleId = user.RoleId,
            Expiration = DateTime.UtcNow.AddHours(1)
        };

        return ResponseFactory.Success(
            response,
            "Login successful",
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
                "Unable to retrieve email from Azure AD token.",
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

    public async Task<BaseResponse<object>> Register(RegisterRequest request)
    {
        var existingUser = await _repository.GetByEmailAsync(request.Email);

        if (existingUser != null)
        {
            return ResponseFactory.Failure<object>(
                "A user with this email already exists.",
                StatusCodes.Status409Conflict
            );
        }

        var user = new User
        {
            RoleId = (int)RoleEnum.USER,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            IsActive = true,
        };

        // Hash password
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _repository.CreateUser(user);

        return ResponseFactory.Success<object>(
            null,
            "User registered successfully.",
            StatusCodes.Status201Created
        );
    }
}