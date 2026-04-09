using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OurWaterAPI.Models;

public partial class ConsumptionDebitRecord
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("inputtedBy")]
    public int InputtedBy { get; set; }

    [Column("correctedBy")]
    public int? CorrectedBy { get; set; }

    [Column("customerId")]
    public int CustomerId { get; set; }

    [Column("debit", TypeName = "decimal(18, 2)")]
    public decimal Debit { get; set; }

    [Column("date")]
    public DateOnly Date { get; set; }

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
    public string ImagePath { get; set; } = null!;

    [Column("location")]
    [StringLength(128)]
    [Unicode(false)]
    public string Location { get; set; } = null!;

    [Column("updatedAt", TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }

    [InverseProperty("ConsumptionRecord")]
    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    [ForeignKey("CorrectedBy")]
    [InverseProperty("ConsumptionDebitRecordCorrectedByNavigations")]
    public virtual User? CorrectingUser { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("ConsumptionDebitRecordCustomers")]
    public virtual User Customer { get; set; } = null!;

    [ForeignKey("InputtedBy")]
    [InverseProperty("ConsumptionDebitRecordInputtedByNavigations")]
    public virtual User InputtingUser { get; set; } = null!;
}
