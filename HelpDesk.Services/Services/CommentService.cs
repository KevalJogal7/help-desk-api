using HelpDesk.Domain.Entities;
using HelpDesk.Repositories.Interfaces;
using HelpDesk.Services.Constants;
using HelpDesk.Services.DTOs.Common;
using HelpDesk.Services.DTOs.TicketDTOs;
using HelpDesk.Services.Enums;
using HelpDesk.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HelpDesk.Services.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _repository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IAuthService _authService;

    public CommentService(
        ICommentRepository repository,
        ITicketRepository ticketRepository,
        IAuthService authService)
    {
        _repository = repository;
        _ticketRepository = ticketRepository;
        _authService = authService;
    }

    public async Task<BaseResponse<object>> AddComment(CommentRequest request)
    {
        Ticket? ticket = await _ticketRepository.GetTicketById(request.TicketId);

        if (ticket == null)
        {
            return ResponseFactory.Failure<object>(
                Messages.General.NotFound,
                StatusCodes.Status404NotFound);
        }

        if (ticket.StatusId == (int)TicketStatusEnum.CLOSED)
        {
            return ResponseFactory.Failure<object>(
                Messages.Ticket.TicketClosed,
                StatusCodes.Status400BadRequest);
        }

        var comment = new TicketComment
        {
            CommentId = Guid.NewGuid(),
            TicketId = request.TicketId,
            Comment = request.Comment,
            CommentBy = _authService.UserId,
            CreatedOn = DateTime.UtcNow
        };

        await _repository.AddComment(comment);

        return ResponseFactory.Success<object>(
            new object(),
            Messages.Comment.AddSuccess,
            StatusCodes.Status201Created);
    }

    public async Task<BaseResponse<object>> DeleteComment(Guid id)
    {
        TicketComment? existing = await _repository.GetCommentById(id);

        if (existing == null)
        {
            return ResponseFactory.Failure<object>(
                Messages.General.NotFound,
                StatusCodes.Status404NotFound);
        }
        Ticket? ticket = await _ticketRepository.GetTicketById(existing.TicketId);

        if (ticket == null)
        {
            return ResponseFactory.Failure<object>(
                Messages.General.NotFound,
                StatusCodes.Status404NotFound);
        }

        if (ticket.StatusId == (int)TicketStatusEnum.CLOSED)
        {
            return ResponseFactory.Failure<object>(
                Messages.Ticket.TicketClosed,
                StatusCodes.Status400BadRequest);
        }

        existing.IsDeleted = true;

        await _repository.UpdateComment(existing);

        return ResponseFactory.Success<object>(
            new object(),
            Messages.General.Success,
            StatusCodes.Status200OK);
    }

    public async Task<BaseResponse<List<CommentResponse>>> GetCommentsByTicketId(Guid ticketId)
    {
        Ticket? ticket = await _ticketRepository.GetTicketById(ticketId);

        if (ticket == null)
        {
            return ResponseFactory.Failure<List<CommentResponse>>(
                Messages.General.NotFound,
                StatusCodes.Status404NotFound);
        }

        List<TicketComment> comments = await _repository.GetCommentsByTicketId(ticketId);

        List<CommentResponse> response = comments.Select(c => new CommentResponse
        {
            CommentId = c.CommentId,
            TicketId = c.TicketId,
            Comment = c.Comment,
            CommentBy = c.CommentByNavigation.Name,
            CreatedOn = c.CreatedOn,
            UpdatedOn = c.UpdatedOn,
            IsOwner = _authService.UserId == c.CommentBy,
            IsDeleted = c.IsDeleted
        }).ToList();

        return ResponseFactory.Success(
            response,
            Messages.General.Success,
            StatusCodes.Status200OK);
    }
}
