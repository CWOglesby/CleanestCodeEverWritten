using System;
using System.Collections.Generic;

namespace paylocity_payroll_api_test01.DataAccess.Model;

public partial class PayRun
{
    public long PayRunId { get; set; }

    public long CompanyId { get; set; }

    public byte PayRunType { get; set; }

    public byte PayRunStatus { get; set; }

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
