using System;
using System.Collections.Generic;

namespace OurWaterAPI.Models;

public partial class Bill
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int ConsumptionRecordId { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!;

    public string RejectionReason { get; set; } = null!;

    public string? ImagePath { get; set; }

    public DateTime Deadline { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ConsumptionDebitRecord ConsumptionRecord { get; set; } = null!;

    public virtual User Customer { get; set; } = null!;

    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();

    public decimal ExtraFine => Fines.Sum(f => f.FineRule.GetFineCost(CreatedAt));

    public decimal TotalPrice => ExtraFine + Amount;
}
