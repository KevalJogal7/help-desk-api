using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Domain.Entities;

public partial class ResetPasswordToken
{
    [Key]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsUsed { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("ResetPasswordTokens")]
    public virtual User User { get; set; } = null!;
}
