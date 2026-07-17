namespace HelpDesk.Services.DTOs.TicketDTOs;

public class StatusUpdateRequest
{
    public Guid TicketId {get; set;}
    public int StatusId {get; set;}
}
