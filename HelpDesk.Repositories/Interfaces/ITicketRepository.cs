using HelpDesk.Domain.Entities;

namespace HelpDesk.Repositories.Interfaces;

public interface ITicketRepository
{
    IQueryable<Ticket> GetList();
    Task<Ticket?> GetTicketById(Guid id);
    Task CreateTicket(Ticket ticket);
    Task UpdateTicket(Ticket ticket);
    Task<List<Category>> GetCategoryList();
    Task<List<SubCategory>> GetSubCategoryList();
    Task<List<TicketStatus>> GetStatusList();
    Task<List<TicketPriority>> GetPriorityList();
}
