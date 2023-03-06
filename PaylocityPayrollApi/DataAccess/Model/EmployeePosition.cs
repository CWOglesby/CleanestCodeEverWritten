using PaylocityPayrollApi.Enums;
using System;
using System.Collections.Generic;

namespace PaylocityPayrollApi.DataAccess.Model;

public partial class EmployeePosition
{
    public long EmployeePositionId { get; set; }

    public long EmployeeId { get; set; }

    public long PositionId { get; set; }

    public PayFrequency PayFrequency { get; set; }

    public decimal PayRate { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Position Position { get; set; } = null!;
}
