using HelpDesk.Domain.Entities;

namespace HelpDesk.Repositories.Interfaces;

public interface ICommentRepository
{
    Task AddComment(TicketComment comment);
    Task UpdateComment(TicketComment comment);
    Task<TicketComment?> GetCommentById(Guid commentId);
    Task<List<TicketComment>> GetCommentsByTicketId(Guid ticketId);
}
