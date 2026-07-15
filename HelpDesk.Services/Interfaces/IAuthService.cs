using HelpDesk.Domain.Entities;
using HelpDesk.Services.DTOs.Common;
using HelpDesk.Services.DTOs.ForgotPasswordDTOs;
using HelpDesk.Services.DTOs.LoginDTOs;
using HelpDesk.Services.DTOs.RegisterDTOs;
using HelpDesk.Services.Enums;

namespace HelpDesk.Services.Interfaces;

public interface IAuthService
{
    Task<BaseResponse<LoginResponse>> Login(LoginRequest request, Boolean isSSO = false);
    Task<BaseResponse<LoginResponse>> SSOLogin(string token);
    Task<BaseResponse<object>> Register(RegisterRequest request);
    Task<BaseResponse<object>> ForgotPassword(ForgotPasswordRequest request);
    string Email { get; }
    RoleEnum Role { get; }
    Guid UserId { get; }
}
