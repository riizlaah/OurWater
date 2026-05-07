using System;
using System.Collections.Generic;

namespace OurWaterAPI.Models;

public partial class Fine
{
    public int Id { get; set; }

    public int BillId { get; set; }

    public int FineRuleId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Bill Bill { get; set; } = null!;

    public virtual FineRule FineRule { get; set; } = null!;
}
