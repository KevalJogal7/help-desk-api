using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Domain.Entities;

public partial class AuditLog
{
    [Key]
    public Guid AuditLogId { get; set; }

    [StringLength(100)]
    public string ModuleName { get; set; } = null!;

    [StringLength(100)]
    public string EntityName { get; set; } = null!;

    public Guid EntityId { get; set; }

    [StringLength(50)]
    public string Action { get; set; } = null!;

    [Column(TypeName = "jsonb")]
    public string? OldValue { get; set; }

    [Column(TypeName = "jsonb")]
    public string? NewValue { get; set; }

    public Guid PerformedBy { get; set; }

    [StringLength(50)]
    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public DateTime PerformedOn { get; set; }

    [ForeignKey("PerformedBy")]
    [InverseProperty("AuditLogs")]
    public virtual User PerformedByNavigation { get; set; } = null!;
}
