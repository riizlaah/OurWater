using System;
using System.Collections.Generic;

namespace OurWaterAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<ConsumptionDebitRecord> ConsumptionDebitRecordCorrectedByNavigations { get; set; } = new List<ConsumptionDebitRecord>();

    public virtual ICollection<ConsumptionDebitRecord> ConsumptionDebitRecordCustomers { get; set; } = new List<ConsumptionDebitRecord>();

    public virtual ICollection<ConsumptionDebitRecord> ConsumptionDebitRecordInputtedByNavigations { get; set; } = new List<ConsumptionDebitRecord>();

    public virtual ICollection<ProductionDebitRecord> ProductionDebitRecords { get; set; } = new List<ProductionDebitRecord>();
}
