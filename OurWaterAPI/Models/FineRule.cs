using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OurWaterAPI.Models;

public partial class FineRule
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("dayAfterDeadline")]
    public int DayAfterDeadline { get; set; }

    [Column("fineAmount", TypeName = "decimal(18, 2)")]
    public decimal FineAmount { get; set; }

    [InverseProperty("FineRule")]
    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();
}
