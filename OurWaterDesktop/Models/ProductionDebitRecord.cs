namespace OurWaterDesktop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductionDebitRecord
    {
        public int id { get; set; }

        public int inputtedBy { get; set; }

        public decimal debit { get; set; }

        [Column(TypeName = "date")]
        public DateTime date { get; set; }

        [Required]
        [StringLength(128)]
        public string location { get; set; }

        public virtual User InputtingUser { get; set; }
    }
}
