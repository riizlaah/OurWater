namespace OurWaterDesktop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Fine
    {
        public int id { get; set; }

        public int billId { get; set; }

        public int fineRuleId { get; set; }

        public DateTime createdAt { get; set; }

        public virtual Bill Bill { get; set; }

        public virtual FineRule FineRule { get; set; }

        public string FineRuleStr => $"{FineRule.dayAfterDeadline} day - {FineRule.fineAmount:Rp#,##0;(Rp#,##0);Rp0}";
    }
}
