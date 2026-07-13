using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Domain.Entities;

public partial class TicketAttachment
{
    [Key]
    public Guid AttachmentId { get; set; }

    public Guid TicketId { get; set; }

    public Guid? CommentId { get; set; }

    [StringLength(255)]
    public string FileName { get; set; } = null!;

    [StringLength(255)]
    public string OriginalFileName { get; set; } = null!;

    [StringLength(20)]
    public string FileExtension { get; set; } = null!;

    [StringLength(100)]
    public string ContentType { get; set; } = null!;

    public long FileSize { get; set; }

    [StringLength(1000)]
    public string FilePath { get; set; } = null!;

    public DateTime UploadedOn { get; set; }

    [ForeignKey("CommentId")]
    [InverseProperty("TicketAttachments")]
    public virtual TicketComment? Comment { get; set; }

    [ForeignKey("TicketId")]
    [InverseProperty("TicketAttachments")]
    public virtual Ticket Ticket { get; set; } = null!;
}
