using System;
using System.Collections.Generic;

namespace PaylocityPayrollApi.DataAccess.Model;

public partial class Position
{
    public long PositionId { get; set; }

    public long CompanyId { get; set; }

    public string Title { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<EmployeePosition> EmployeePositions { get; } = new List<EmployeePosition>();
}
