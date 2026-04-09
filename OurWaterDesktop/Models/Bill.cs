namespace OurWaterDesktop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Bill
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bill()
        {
            Fines = new HashSet<Fine>();
        }

        [Key]
        public int id { get; set; }

        public int customerId { get; set; }

        public int consumptionRecordId { get; set; }

        public decimal amount { get; set; }

        [Required]
        [StringLength(16)]
        public string status { get; set; }

        public string rejectionReason { get; set; }

        public string imagePath { get; set; } = null;

        public DateTime deadline { get; set; }

        public DateTime updatedAt { get; set; }

        public DateTime createdAt { get; set; }

        public virtual ConsumptionDebitRecord ConsumptionDebitRecord { get; set; }

        public virtual User Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fine> Fines { get; set; }
    }
}
