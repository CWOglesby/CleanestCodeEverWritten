using System;
using System.Collections.Generic;

namespace paylocity_payroll_api_test01.DataAccess.Model;

public partial class EmployeePosition
{
    public long EmployeePositionId { get; set; }

    public long EmployeeId { get; set; }

    public long PositionId { get; set; }

    public byte PayFrequency { get; set; }

    public decimal PayRate { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Position Position { get; set; } = null!;
}
