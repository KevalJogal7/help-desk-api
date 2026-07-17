namespace HelpDesk.API.Controllers;

using HelpDesk.Services.DTOs.ForgotPasswordDTOs;
using HelpDesk.Services.DTOs.LoginDTOs;
using HelpDesk.Services.DTOs.ProfileDTOs;
using HelpDesk.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.Login(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("sso-login")]
    public async Task<IActionResult> SSOLogin(SSOLoginRequest request)
    {
        var response = await _authService.SSOLogin(request.Token);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        var response = await _authService.ForgotPassword(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var response = await _authService.GetProfile();
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request)
    {
        var response = await _authService.UpdateProfile(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var response = await _authService.ChangePassword(request);
        return StatusCode(response.StatusCode, response);
    }
}