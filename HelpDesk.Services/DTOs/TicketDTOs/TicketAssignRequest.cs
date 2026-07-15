namespace HelpDesk.Services.DTOs.TicketDTOs;

public class TicketAssignRequest
{
    public Guid TicketId {get; set;}
    public Guid AssignedTo {get; set;}
}
