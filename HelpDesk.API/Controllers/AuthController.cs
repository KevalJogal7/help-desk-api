using System.Security.Claims;
using HelpDesk.Domain.Entities;
using HelpDesk.Services.DTOs;
using HelpDesk.Services.Interfaces;
using HelpDesk.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly AzureTokenValidator _azureTokenValidator;

    public AuthController(IAuthService authService, AzureTokenValidator azureTokenValidator)
    {
        _authService = authService;
        _azureTokenValidator = azureTokenValidator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.Login(request);

        if (response == null) return Unauthorized();

        return Ok(response);
    }

    [HttpPost("sso-login")]
    public async Task<IActionResult> SSOLogin(SSOLoginRequest request)
    {
        ClaimsPrincipal principal = await _azureTokenValidator.ValidateAsync(request.token);
        var email = principal.FindFirst("preferred_username")?.Value;
        LoginRequest req = new LoginRequest()
        {
            Email = email,
            Password = ""
        };

        var response = await _authService.Login(req, true);

        if (response == null) return Unauthorized();

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        User? response = await _authService.Register(request);

        if (response == null) return Unauthorized();
        return Ok();
    }
}