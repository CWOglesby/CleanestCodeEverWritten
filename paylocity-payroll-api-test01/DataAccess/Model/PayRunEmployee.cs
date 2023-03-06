using System;
using System.Collections.Generic;

namespace paylocity_payroll_api_test01.DataAccess.Model;

public partial class PayRunEmployee
{
    public long PayRunEmployeeId { get; set; }

    public long PayRunId { get; set; }

    public long EmployeeId { get; set; }

    public decimal EarningsAmount { get; set; }

    public decimal DeductionsAmount { get; set; }

    public decimal WithholdingsAmount { get; set; }

    public decimal NetAmount { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual PayRun PayRun { get; set; } = null!;

    public virtual ICollection<PayRunDetail> PayRunDetails { get; } = new List<PayRunDetail>();
}
