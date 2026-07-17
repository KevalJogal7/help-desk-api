namespace HelpDesk.Repositories.Repositories;

using HelpDesk.Domain.Context;
using HelpDesk.Domain.Entities;
using HelpDesk.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _context;

    public CommentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddComment(TicketComment comment)
    {
        await _context.TicketComments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateComment(TicketComment comment)
    {
        _context.TicketComments.Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<TicketComment?> GetCommentById(Guid commentId)
    {
        return await _context.TicketComments
            .Include(c => c.CommentByNavigation)
            .FirstOrDefaultAsync(c => c.CommentId == commentId);
    }

    public async Task<List<TicketComment>> GetCommentsByTicketId(Guid ticketId)
    {
        return await _context.TicketComments
            .Include(c => c.CommentByNavigation)
            .Where(c => c.TicketId == ticketId)
            .OrderBy(c => c.CreatedOn)
            .ToListAsync();
    }
}
