namespace HelpDesk.Repositories.Repositories;

using HelpDesk.Domain.Context;
using HelpDesk.Domain.Entities;
using HelpDesk.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<Ticket> GetList()
    {
        return _context.Tickets
            .Include(t => t.CreatedByNavigation)
            .Include(t => t.AssignedToNavigation)
            .Include(t => t.Priority)
            .Include(t => t.Status)
            .Include(t => t.Category)
            .Include(t => t.SubCategory)
            .AsQueryable();
    }

    public Task<Ticket?> GetTicketById(Guid id)
    {
        return _context.Tickets
            .Include(t => t.CreatedByNavigation)
            .Include(t => t.AssignedToNavigation)
            .Include(t => t.Priority)
            .Include(t => t.Status)
            .Include(t => t.Category)
            .Include(t => t.SubCategory)
            .FirstOrDefaultAsync(t => t.TicketId == id);
    }

    public async Task CreateTicket(Ticket ticket)
    {
        await _context.Tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTicket(Ticket ticket)
    {
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Category>> GetCategoryList()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<List<SubCategory>> GetSubCategoryList()
    {
        return await _context.SubCategories.ToListAsync();
    }

    public async Task<List<TicketStatus>> GetStatusList()
    {
        return await _context.TicketStatuses.ToListAsync();
    }

    public async Task<List<TicketPriority>> GetPriorityList()
    {
        return await _context.TicketPriorities.ToListAsync();
    }

}