using System;
using System.Collections.Generic;

namespace OurWaterAPI.Models;

public partial class FineRule
{
    public int Id { get; set; }

    public int StartDay { get; set; }

    public int? EndDay { get; set; }

    public decimal FineAmount { get; set; }

    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();
}
