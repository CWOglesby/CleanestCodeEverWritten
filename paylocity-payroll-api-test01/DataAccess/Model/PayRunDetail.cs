using System;
using System.Collections.Generic;

namespace paylocity_payroll_api_test01.DataAccess.Model;

public partial class PayRunDetail
{
    public long PayRunDetailId { get; set; }

    public long PayRunEmployeeId { get; set; }

    public byte PayRunDetailType { get; set; }

    public decimal Amount { get; set; }

    public virtual PayRunEmployee PayRunEmployee { get; set; } = null!;
}
