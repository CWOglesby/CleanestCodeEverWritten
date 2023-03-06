using PaylocityPayrollApi.Enums;
using System;
using System.Collections.Generic;

namespace PaylocityPayrollApi.DataAccess.Model;

public partial class BenefitDiscount
{
    public long BenefitDiscountId { get; set; }

    public long BenefitId { get; set; }

    public string FilterByColumn { get; set; } = null!;

    public string FilterPattern { get; set; } = null!;

    public BenefitDiscountType DiscountType { get; set; }

    public decimal DiscountAmount { get; set; }

    public virtual Benefit Benefit { get; set; } = null!;
}
