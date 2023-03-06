using paylocity_payroll_api_test01.Enums;
using System;
using System.Collections.Generic;

namespace paylocity_payroll_api_test01.DataAccess.Model;

public partial class Benefit
{
    public long BenefitId { get; set; }

    public long CompanyId { get; set; }

    public BenefitType BenefitType { get; set; }

    public string Description { get; set; } = null!;

    public decimal AnnualCostEmployee { get; set; }

    public decimal AnnualCostDependent { get; set; }

    public DateTime EffectiveDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual ICollection<BenefitDiscount> BenefitDiscounts { get; } = new List<BenefitDiscount>();

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<EnrollmentBenefit> EnrollmentBenefits { get; } = new List<EnrollmentBenefit>();
}
