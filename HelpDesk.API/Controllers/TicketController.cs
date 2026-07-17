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

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _ticketService.GetTicketById(id);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("upsert")]
    [Authorize]
    public async Task<IActionResult> UpsertTicket(UpsertTicketRequest request)
    {
        var response = await _ticketService.UpsertTicket(request);

        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteTicket(Guid id)
    {
        var response = await _ticketService.DeleteTicket(id);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("assign")]
    [Authorize(Roles = nameof(RoleEnum.ADMIN))]
    public async Task<IActionResult> AssignTicket(TicketAssignRequest request)
    {
        var response = await _ticketService.AssignTicket(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("status-change")]
    [Authorize(Roles = $"{nameof(RoleEnum.ADMIN)},{nameof(RoleEnum.SUPPORT_AGENT)}")]
    public async Task<IActionResult> StatusUpdate(StatusUpdateRequest request)
    {
        var response = await _ticketService.StatusUpdate(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("category-list")]
    [Authorize]
    public async Task<IActionResult> GetCategoryList()
    {
        var response = await _ticketService.GetCategoryList();

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("sub-category-list")]
    [Authorize]
    public async Task<IActionResult> GetSubCategoryList()
    {
        var response = await _ticketService.GetSubCategoryList();

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("status-list")]
    [Authorize]
    public async Task<IActionResult> GetStatusList()
    {
        var response = await _ticketService.GetStatusList();

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("priority-list")]
    [Authorize]
    public async Task<IActionResult> GetPriorityList()
    {
        var response = await _ticketService.GetPriorityList();

        return StatusCode(response.StatusCode, response);
    }
}