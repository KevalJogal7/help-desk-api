namespace HelpDesk.Services.DTOs.TicketDTOs;

using System.ComponentModel.DataAnnotations;
using HelpDesk.Services.Constants;

public class UpsertTicketRequest
{
    // Null on create, required on update
    public Guid? TicketId { get; set; }

    [Required(ErrorMessage = Messages.Ticket.TitleRequired)]
    [StringLength(250, MinimumLength = 3, ErrorMessage = Messages.Ticket.TitleLength)]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = Messages.Ticket.DescriptionRequired)]
    public string Description { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = Messages.Ticket.PriorityRequired)]
    public int PriorityId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = Messages.Ticket.CategoryRequired)]
    public int CategoryId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = Messages.Ticket.SubCategoryRequired)]
    public int SubCategoryId { get; set; }

    public int StatusId { get; set; }

    public Guid? AssignedTo { get; set; } = null;
}
