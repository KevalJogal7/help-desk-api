namespace HelpDesk.API.Controllers;

using HelpDesk.Services.DTOs.UserDTOs;
using HelpDesk.Services.Interfaces;
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
    public async Task<IActionResult> UserList(UserRequest request)
    {
        var response = await _userService.GetUserList(request);

        return StatusCode(response.StatusCode, response);
    }

}