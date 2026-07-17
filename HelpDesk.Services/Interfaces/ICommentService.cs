using HelpDesk.Services.DTOs.Common;
using HelpDesk.Services.DTOs.TicketDTOs;

namespace HelpDesk.Services.Interfaces;

public interface ICommentService
{
    Task<BaseResponse<object>> AddComment(CommentRequest request);
    Task<BaseResponse<object>> DeleteComment(Guid id);
    Task<BaseResponse<List<CommentResponse>>> GetCommentsByTicketId(Guid ticketId);
}
