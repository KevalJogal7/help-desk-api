using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Domain.Entities;

public partial class TicketComment
{
    [Key]
    public Guid CommentId { get; set; }

    public Guid TicketId { get; set; }

    public Guid CommentBy { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    [ForeignKey("CommentBy")]
    [InverseProperty("TicketComments")]
    public virtual User CommentByNavigation { get; set; } = null!;

    [ForeignKey("TicketId")]
    [InverseProperty("TicketComments")]
    public virtual Ticket Ticket { get; set; } = null!;

    public bool IsDeleted { get; set; }

    [InverseProperty("Comment")]
    public virtual ICollection<TicketAttachment> TicketAttachments { get; set; } = new List<TicketAttachment>();
}
