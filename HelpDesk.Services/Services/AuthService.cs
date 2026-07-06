using HelpDesk.Domain.Entities;
using HelpDesk.Repositories.Interfaces;
using HelpDesk.Services.DTOs;
using HelpDesk.Services.Enums;
using HelpDesk.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HelpDesk.Services.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IJwtService _jwtService;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(IUserRepository repository, IJwtService jwtService)
    {
        _passwordHasher = new PasswordHasher<User>();
        _repository = repository;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse?> Login(LoginRequest request, Boolean isSSO = false)
    {
        User? user = await _repository.GetByEmailAsync(request.Email);

        if (user == null)
            return null;

        if (!user.IsActive || user.IsDeleted) return null;

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (!isSSO && result == PasswordVerificationResult.Failed) return null;

        var token = _jwtService.GenerateToken(user);

        return new LoginResponse
        {
            AccessToken = token,
            RefreshToken = "",
            UserName = user.FirstName + " " + user.LastName,
            RoleId = user.RoleId,
            Expiration = DateTime.Now.AddHours(1)
        };
    }

    public async Task<User?> Register(RegisterRequest request)
    {
        User user = new User()
        { 
            RoleId = (int)RoleEnum.USER,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            IsActive = true,
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
        return await _repository.CreateUser(user);
    }
}