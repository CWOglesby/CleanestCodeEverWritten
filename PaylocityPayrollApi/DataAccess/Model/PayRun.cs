﻿using PaylocityPayrollApi.Enums;
using System;
using System.Collections.Generic;

namespace PaylocityPayrollApi.DataAccess.Model;

public partial class PayRun
{
    public long PayRunId { get; set; }

    public long CompanyId { get; set; }

    public PayRunType PayRunType { get; set; }

    public PayRunStatus PayRunStatus { get; set; }

    public decimal EarningsTotal { get; set; }

    public decimal DeductionsTotal { get; set; }

    public decimal WithholdingsTotal { get; set; }

    public decimal NetTotal { get; set; }

    public DateTime PayPeriodFrom { get; set; }

    public DateTime PayPeriodTo { get; set; }

    public DateTime PayDate { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<PayRunEmployee> PayRunEmployees { get; } = new List<PayRunEmployee>();
}
