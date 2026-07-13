namespace HelpDesk.Services.DTOs.TicketDTOs;

public class TicketResponse
{
    public Guid TicketId { get; set; }

    public string TicketNumber { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string Priority { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string SubCategory { get; set; } = null!;
}
