using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Domain.Entities;

public partial class Category
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedOn { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();

    [InverseProperty("Category")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
