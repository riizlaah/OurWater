using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OurWaterAPI.Models;

public partial class Fine
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("billId")]
    public int BillId { get; set; }

    [Column("fineRuleId")]
    public int FineRuleId { get; set; }

    [Column("createdAt", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("BillId")]
    [InverseProperty("Fines")]
    public virtual Bill Bill { get; set; } = null!;

    [ForeignKey("FineRuleId")]
    [InverseProperty("Fines")]
    public virtual FineRule FineRule { get; set; } = null!;
}
