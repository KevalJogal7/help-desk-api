namespace HelpDesk.API.Controllers;

using HelpDesk.Services.DTOs.TicketDTOs;
using HelpDesk.Services.Enums;
using HelpDesk.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpPost("list")]
    [Authorize]
    public async Task<IActionResult> GetList(TicketRequest request)
    {
        var response = await _ticketService.GetList(request);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("category-list")]
    public async Task<IActionResult> GetCategoryList()
    {
        var response = await _ticketService.GetCategoryList();

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("sub-category-list")]
    public async Task<IActionResult> GetSubCategoryList()
    {
        var response = await _ticketService.GetSubCategoryList();

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("status-list")]
    public async Task<IActionResult> GetStatusList()
    {
        var response = await _ticketService.GetStatusList();

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("priority-list")]
    public async Task<IActionResult> GetPriorityList()
    {
        var response = await _ticketService.GetPriorityList();

        return StatusCode(response.StatusCode, response);
    }
}