using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Domain.Entities;

public partial class SubCategory
{
    [Key]
    public int Id { get; set; }

    public int CategoryId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedOn { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("SubCategories")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("SubCategory")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
