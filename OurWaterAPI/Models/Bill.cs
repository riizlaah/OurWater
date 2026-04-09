using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OurWaterAPI.Models;

public partial class Bill
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("customerId")]
    public int CustomerId { get; set; }

    [Column("consumptionRecordId")]
    public int ConsumptionRecordId { get; set; }

    [Column("amount", TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [Column("status")]
    [StringLength(16)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Column("rejectionReason")]
    [StringLength(300)]
    [Unicode(false)]
    public string RejectionReason { get; set; } = null!;

    [Column("imagePath")]
    [StringLength(128)]
    [Unicode(false)]
    public string? ImagePath { get; set; }

    [Column("deadline", TypeName = "datetime")]
    public DateTime Deadline { get; set; }

    [Column("updatedAt", TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }

    [Column("createdAt", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("ConsumptionRecordId")]
    [InverseProperty("Bills")]
    public virtual ConsumptionDebitRecord ConsumptionRecord { get; set; } = null!;

    [ForeignKey("CustomerId")]
    [InverseProperty("Bills")]
    public virtual User Customer { get; set; } = null!;

    [InverseProperty("Bill")]
    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();
}
