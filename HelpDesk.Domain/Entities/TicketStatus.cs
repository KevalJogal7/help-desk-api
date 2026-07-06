using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Domain.Entities;

[Table("TicketStatus")]
[Index("StatusName", Name = "uq_ticket_status_name", IsUnique = true)]
public partial class TicketStatus
{
    [StringLength(50)]
    public string StatusName { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    public short SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    [Key]
    public int StatusId { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
