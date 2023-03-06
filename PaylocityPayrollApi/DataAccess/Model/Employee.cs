using System;
using System.Collections.Generic;

namespace paylocity_payroll_api_test01.DataAccess.Model;

public partial class Employee
{
    public long EmployeeId { get; set; }

    public long CompanyId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<EmployeeEnrollment> EmployeeEnrollments { get; } = new List<EmployeeEnrollment>();

    public virtual ICollection<EmployeePosition> EmployeePositions { get; } = new List<EmployeePosition>();

    public virtual ICollection<PayRunEmployee> PayRunEmployees { get; } = new List<PayRunEmployee>();
}
