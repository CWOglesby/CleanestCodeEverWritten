using paylocity_payroll_api_test01.Enums;
using System;
using System.Collections.Generic;

namespace paylocity_payroll_api_test01.DataAccess.Model;

public partial class EmployeeEnrollment
{
    public long EmployeeEnrollmentId { get; set; }

    public long EmployeeId { get; set; }

    public EnrollmentEventType EnrollmentEventType { get; set; }

    public DateTime EffectiveDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<EnrollmentBenefit> EnrollmentBenefits { get; } = new List<EnrollmentBenefit>();

    public virtual ICollection<EnrollmentDependent> EnrollmentDependents { get; } = new List<EnrollmentDependent>();
}
