using HelpDesk.Domain.Entities;
using HelpDesk.Services.DTOs;

namespace HelpDesk.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> Login(LoginRequest request, Boolean isSSO = false);
    Task<User?> Register(RegisterRequest request);
}
