using HelpDesk.Domain.Entities;
using HelpDesk.Services.DTOs;
using HelpDesk.Services.DTOs.Common;

namespace HelpDesk.Services.Interfaces;

public interface IAuthService
{
    Task<BaseResponse<LoginResponse>> Login(LoginRequest request, Boolean isSSO = false);
    Task<BaseResponse<LoginResponse>> SSOLogin(string token);
    Task<BaseResponse<object>> Register(RegisterRequest request);
    Task<BaseResponse<object>> ForgotPassword(ForgotPasswordRequest request);
}
