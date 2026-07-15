using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Domain.Entities;

[Index("TicketNumber", Name = "uq_tickets_ticket_number", IsUnique = true)]
public partial class Ticket
{
    [Key]
    public Guid TicketId { get; set; }

    [StringLength(30)]
    public string TicketNumber { get; set; } = null!;

    [StringLength(250)]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public DateTime? ClosedDate { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public int PriorityId { get; set; }

    public int StatusId { get; set; }

    public int CategoryId { get; set; }

    public int SubCategoryId { get; set; }

    public Guid? AssignedTo { get; set; }

    public Guid? AssignedBy { get; set; }

    [ForeignKey("AssignedBy")]
    [InverseProperty("TicketAssignedByNavigations")]
    public virtual User? AssignedByNavigation { get; set; }

    [ForeignKey("AssignedTo")]
    [InverseProperty("TicketAssignedToNavigations")]
    public virtual User? AssignedToNavigation { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Tickets")]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("CreatedBy")]
    [InverseProperty("TicketCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [ForeignKey("PriorityId")]
    [InverseProperty("Tickets")]
    public virtual TicketPriority Priority { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("Tickets")]
    public virtual TicketStatus Status { get; set; } = null!;

    [ForeignKey("SubCategoryId")]
    [InverseProperty("Tickets")]
    public virtual SubCategory SubCategory { get; set; } = null!;

    [InverseProperty("Ticket")]
    public virtual ICollection<TicketAttachment> TicketAttachments { get; set; } = new List<TicketAttachment>();

    [InverseProperty("Ticket")]
    public virtual ICollection<TicketComment> TicketComments { get; set; } = new List<TicketComment>();

    [InverseProperty("Ticket")]
    public virtual ICollection<TicketHistory> TicketHistories { get; set; } = new List<TicketHistory>();

    [ForeignKey("UpdatedBy")]
    [InverseProperty("TicketUpdatedByNavigations")]
    public virtual User? UpdatedByNavigation { get; set; }
}
