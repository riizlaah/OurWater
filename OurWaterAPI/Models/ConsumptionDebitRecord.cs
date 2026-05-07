using System;
using System.Collections.Generic;

namespace OurWaterAPI.Models;

public partial class ConsumptionDebitRecord
{
    public int Id { get; set; }

    public int InputtedBy { get; set; }

    public int? CorrectedBy { get; set; }

    public int CustomerId { get; set; }

    public decimal Debit { get; set; }

    public DateOnly Date { get; set; }

    public string Status { get; set; } = null!;

    public string RejectionReason { get; set; } = null!;

    public string ImagePath { get; set; } = null!;

    public string Location { get; set; } = null!;

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual User? CorrectedByNavigation { get; set; }

    public virtual User Customer { get; set; } = null!;

    public virtual User InputtedByNavigation { get; set; } = null!;
}
