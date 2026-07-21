namespace HelpDesk.API.Controllers;

using HelpDesk.Services.Enums;
using HelpDesk.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    [Authorize(Roles = nameof(RoleEnum.ADMIN))]
    public async Task<IActionResult> GetDashboard()
    {
        var response = await _dashboardService.GetDashboard();
        return StatusCode(response.StatusCode, response);
    }
}