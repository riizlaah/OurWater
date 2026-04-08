namespace OurWaterDesktop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ConsumptionDebitRecord
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConsumptionDebitRecord()
        {
            Bills = new HashSet<Bill>();
        }

        public int id { get; set; }

        public int inputtedBy { get; set; }

        public int? correctedBy { get; set; }

        public int customerId { get; set; }

        public decimal debit { get; set; }

        [Column(TypeName = "date")]
        public DateTime date { get; set; }

        [Required]
        [StringLength(16)]
        public string status { get; set; }

        [Required]
        [StringLength(300)]
        public string rejectionReason { get; set; }

        [Required]
        [StringLength(128)]
        public string imagePath { get; set; }

        [Required]
        [StringLength(128)]
        public string location { get; set; }

        public DateTime updatedAt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bill> Bills { get; set; }

        public virtual User InputtingUser { get; set; }

        public virtual User Customer { get; set; }

        public virtual User CorrectingUser { get; set; }
    }
}
