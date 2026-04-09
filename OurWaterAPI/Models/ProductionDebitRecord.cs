using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OurWaterAPI.Models;

public partial class ProductionDebitRecord
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("inputtedBy")]
    public int InputtedBy { get; set; }

    [Column("debit", TypeName = "decimal(18, 2)")]
    public decimal Debit { get; set; }

    [Column("date")]
    public DateOnly Date { get; set; }

    [Column("location")]
    [StringLength(128)]
    [Unicode(false)]
    public string Location { get; set; } = null!;

    [ForeignKey("InputtedBy")]
    [InverseProperty("ProductionDebitRecords")]
    public virtual User InputtedByNavigation { get; set; } = null!;
}
