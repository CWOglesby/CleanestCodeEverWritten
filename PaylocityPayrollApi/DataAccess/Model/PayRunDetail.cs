using PaylocityPayrollApi.Enums;
using System;
using System.Collections.Generic;

namespace PaylocityPayrollApi.DataAccess.Model;

public partial class PayRunDetail
{
    public long PayRunDetailId { get; set; }

    public long PayRunEmployeeId { get; set; }

    public PayRunDetailType PayRunDetailType { get; set; }

    public decimal Amount { get; set; }

    public virtual PayRunEmployee PayRunEmployee { get; set; } = null!;
}
