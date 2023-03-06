using System;
using System.Collections.Generic;

namespace paylocity_payroll_api_test01.DataAccess.Model;

public partial class Company
{
    public long CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Benefit> Benefits { get; } = new List<Benefit>();

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();

    public virtual ICollection<PayRun> PayRuns { get; } = new List<PayRun>();

    public virtual ICollection<Position> Positions { get; } = new List<Position>();
}
