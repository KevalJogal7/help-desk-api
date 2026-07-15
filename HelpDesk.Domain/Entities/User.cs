using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Domain.Entities;

[Index("Email", Name = "uq_users_email", IsUnique = true)]
public partial class User
{
    [Key]
    public Guid UserId { get; set; }

    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [StringLength(255)]
    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public Guid? AzureObjectId { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int RoleId { get; set; }

    [InverseProperty("PerformedByNavigation")]
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("AssignedByNavigation")]
    public virtual ICollection<Ticket> TicketAssignedByNavigations { get; set; } = new List<Ticket>();

    [InverseProperty("AssignedToNavigation")]
    public virtual ICollection<Ticket> TicketAssignedToNavigations { get; set; } = new List<Ticket>();

    [InverseProperty("CommentByNavigation")]
    public virtual ICollection<TicketComment> TicketComments { get; set; } = new List<TicketComment>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Ticket> TicketCreatedByNavigations { get; set; } = new List<Ticket>();

    [InverseProperty("ChangedByNavigation")]
    public virtual ICollection<TicketHistory> TicketHistories { get; set; } = new List<TicketHistory>();

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<Ticket> TicketUpdatedByNavigations { get; set; } = new List<Ticket>();
}
