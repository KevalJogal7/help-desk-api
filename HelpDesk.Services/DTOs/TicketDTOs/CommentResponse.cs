namespace HelpDesk.Services.DTOs.TicketDTOs;

public class CommentResponse
{
    public Guid CommentId { get; set; }
    public Guid TicketId { get; set; }
    public string Comment { get; set; } = null!;
    public string CommentBy { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public bool IsOwner { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
}
