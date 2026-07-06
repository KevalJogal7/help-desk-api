using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Domain.Entities;

[Table("TicketPriority")]
[Index("PriorityName", Name = "uq_ticket_priority_name", IsUnique = true)]
public partial class TicketPriority
{
    [StringLength(50)]
    public string PriorityName { get; set; } = null!;

    public short SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    [Key]
    public int PriorityId { get; set; }

    [InverseProperty("Priority")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
