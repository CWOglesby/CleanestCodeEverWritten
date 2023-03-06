using System;
using System.Collections.Generic;

namespace PaylocityPayrollApi.DataAccess.Model;

public partial class EnrollmentBenefit
{
    public long EnrollmentBenefitId { get; set; }

    public long EmployeeEnrollmentId { get; set; }

    public long BenefitId { get; set; }

    public virtual Benefit Benefit { get; set; } = null!;

    public virtual EmployeeEnrollment EmployeeEnrollment { get; set; } = null!;
}
