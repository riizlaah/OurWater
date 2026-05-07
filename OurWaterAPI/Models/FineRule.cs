using System;
using System.Collections.Generic;

namespace OurWaterAPI.Models;

public partial class FineRule
{
    public int Id { get; set; }

    public int StartDay { get; set; }

    public int? EndDay { get; set; }

    public string EndDayStr => EndDay.HasValue ? EndDay.Value.ToString() : "...";

    public decimal FineAmount { get; set; }

    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();

    public decimal GetFineCost(DateTime dateTime)
    {
        var totalDays = (DateTime.Now - dateTime).TotalDays;
        var dayPassed = (decimal)totalDays - StartDay;
        dayPassed = EndDay.HasValue ? Math.Min(dayPassed, EndDay.Value) : dayPassed;
        return dayPassed * FineAmount;
    }

    public string DisplayStr => $"{StartDay}-{EndDayStr} day = {FineAmount:Rp#,##0;(Rp#,##0);Rp0} / day";
}
