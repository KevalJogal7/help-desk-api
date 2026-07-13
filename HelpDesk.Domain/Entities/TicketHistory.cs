using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Domain.Entities;

[Table("TicketHistory")]
public partial class TicketHistory
{
    [Key]
    public Guid HistoryId { get; set; }

    public Guid TicketId { get; set; }

    [StringLength(100)]
    public string FieldName { get; set; } = null!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public Guid ChangedBy { get; set; }

    public DateTime ChangedOn { get; set; }

    [StringLength(500)]
    public string? Remarks { get; set; }

    [ForeignKey("ChangedBy")]
    [InverseProperty("TicketHistories")]
    public virtual User ChangedByNavigation { get; set; } = null!;

    [ForeignKey("TicketId")]
    [InverseProperty("TicketHistories")]
    public virtual Ticket Ticket { get; set; } = null!;
}
