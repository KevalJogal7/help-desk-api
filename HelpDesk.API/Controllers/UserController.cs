namespace HelpDesk.API.Controllers;

using HelpDesk.Services.DTOs.UserDTOs;
using HelpDesk.Services.Enums;
using HelpDesk.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("list")]
    public async Task<IActionResult> GetList(UserRequest request)
    {
        var response = await _userService.GetUserList(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = nameof(RoleEnum.ADMIN))]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _userService.GetUserById(id);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("upsert")]
    [Authorize(Roles = nameof(RoleEnum.ADMIN))]
    public async Task<IActionResult> UpsertUser(UpsertUserRequest request)
    {
        var response = await _userService.UpsertUser(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:guid}/toggle-status")]
    [Authorize(Roles = nameof(RoleEnum.ADMIN))]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var response = await _userService.ToggleUserStatus(id);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/{id:guid}")]
    [Authorize(Roles = nameof(RoleEnum.ADMIN))]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var response = await _userService.DeleteUser(id);
        return StatusCode(response.StatusCode, response);
    }
}
