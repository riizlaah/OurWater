using System;
using System.Collections.Generic;

namespace OurWaterAPI.Models;

public partial class ProductionDebitRecord
{
    public int Id { get; set; }

    public int InputtedBy { get; set; }

    public decimal Debit { get; set; }

    public DateOnly Date { get; set; }

    public string Location { get; set; } = null!;

    public virtual User InputtedByNavigation { get; set; } = null!;
}
