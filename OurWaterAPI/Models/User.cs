using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OurWaterAPI.Models;

public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("password")]
    [StringLength(256)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [Column("fullname")]
    [StringLength(50)]
    [Unicode(false)]
    public string Fullname { get; set; } = null!;

    [Column("role")]
    [StringLength(16)]
    [Unicode(false)]
    public string Role { get; set; } = null!;

    [Column("address")]
    [StringLength(128)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    [InverseProperty("Customer")]
    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    [InverseProperty("CorrectingUser")]
    public virtual ICollection<ConsumptionDebitRecord> ConsumptionDebitRecordCorrectedByNavigations { get; set; } = new List<ConsumptionDebitRecord>();

    [InverseProperty("Customer")]
    public virtual ICollection<ConsumptionDebitRecord> ConsumptionDebitRecordCustomers { get; set; } = new List<ConsumptionDebitRecord>();

    [InverseProperty("InputtingUser")]
    public virtual ICollection<ConsumptionDebitRecord> ConsumptionDebitRecordInputtedByNavigations { get; set; } = new List<ConsumptionDebitRecord>();

    [InverseProperty("InputtedByNavigation")]
    public virtual ICollection<ProductionDebitRecord> ProductionDebitRecords { get; set; } = new List<ProductionDebitRecord>();
}
