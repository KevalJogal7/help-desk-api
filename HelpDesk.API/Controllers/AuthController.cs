namespace HelpDesk.API.Controllers;

using HelpDesk.Domain.Entities;
using HelpDesk.Services.DTOs;
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

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var response = await _authService.Register(request);

        return StatusCode(response.StatusCode, response);
    }
}