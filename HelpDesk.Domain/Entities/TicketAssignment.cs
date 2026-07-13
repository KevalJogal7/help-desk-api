using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Domain.Entities;

public partial class TicketAssignment
{
    [Key]
    public Guid AssignmentId { get; set; }

    public Guid TicketId { get; set; }

    public Guid AssignedBy { get; set; }

    public Guid AssignedTo { get; set; }

    [StringLength(500)]
    public string? Remarks { get; set; }

    public DateTime AssignedOn { get; set; }

    [ForeignKey("AssignedBy")]
    [InverseProperty("TicketAssignmentAssignedByNavigations")]
    public virtual User AssignedByNavigation { get; set; } = null!;

    [ForeignKey("AssignedTo")]
    [InverseProperty("TicketAssignmentAssignedToNavigations")]
    public virtual User AssignedToNavigation { get; set; } = null!;

    [ForeignKey("TicketId")]
    [InverseProperty("TicketAssignments")]
    public virtual Ticket Ticket { get; set; } = null!;
}
