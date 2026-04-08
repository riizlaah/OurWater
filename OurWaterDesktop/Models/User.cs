namespace OurWaterDesktop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Bills = new HashSet<Bill>();
            ConsumptionDebitRecords = new HashSet<ConsumptionDebitRecord>();
            ConsumptionDebitRecords1 = new HashSet<ConsumptionDebitRecord>();
            ConsumptionDebitRecords2 = new HashSet<ConsumptionDebitRecord>();
            ProductionDebitRecords = new HashSet<ProductionDebitRecord>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string username { get; set; }

        [Required]
        [StringLength(256)]
        public string password { get; set; }

        [Required]
        [StringLength(50)]
        public string fullname { get; set; }

        [Required]
        [StringLength(16)]
        public string role { get; set; }

        [Required]
        [StringLength(128)]
        public string address { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bill> Bills { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConsumptionDebitRecord> ConsumptionDebitRecords { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConsumptionDebitRecord> ConsumptionDebitRecords1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConsumptionDebitRecord> ConsumptionDebitRecords2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductionDebitRecord> ProductionDebitRecords { get; set; }
    }
}
