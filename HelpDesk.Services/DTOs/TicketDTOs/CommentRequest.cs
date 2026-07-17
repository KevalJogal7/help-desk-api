namespace HelpDesk.Services.DTOs.TicketDTOs;

using System.ComponentModel.DataAnnotations;
using HelpDesk.Services.Constants;

public class CommentRequest
{
    public Guid TicketId { get; set; }

    [Required(ErrorMessage = Messages.Comment.CommentRequired)]
    [StringLength(2000, MinimumLength = 1, ErrorMessage = Messages.Comment.CommentLength)]
    public string Comment { get; set; } = null!;
}
