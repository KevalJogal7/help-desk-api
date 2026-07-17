namespace HelpDesk.API.Controllers;

using HelpDesk.Services.DTOs.TicketDTOs;
using HelpDesk.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost("add")]
    [Authorize]
    public async Task<IActionResult> AddComment(CommentRequest request)
    {
        var response = await _commentService.AddComment(request);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("list/{ticketId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetComments(Guid ticketId)
    {
        var response = await _commentService.GetCommentsByTicketId(ticketId);

        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete/{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        var response = await _commentService.DeleteComment(id);

        return StatusCode(response.StatusCode, response);
    }
}
