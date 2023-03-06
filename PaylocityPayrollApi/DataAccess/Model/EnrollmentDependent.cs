using System;
using System.Collections.Generic;

namespace paylocity_payroll_api_test01.DataAccess.Model;

public partial class EnrollmentDependent
{
    public long EnrollmentDependentId { get; set; }

    public long EmployeeEnrollmentId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual EmployeeEnrollment EmployeeEnrollment { get; set; } = null!;
}
