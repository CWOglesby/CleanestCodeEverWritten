using PaylocityPayrollApi.DataAccess.Model;
using PaylocityPayrollApi.DataAccess.Repository;
using PaylocityPayrollApi.Enums;
using System.Diagnostics.Tracing;
using System.Text.RegularExpressions;

namespace PaylocityPayrollApi.Services.Payroll
{
    public class CalculatePayrollService
    {
        private readonly PayRunRepository _payRunRepository;

        public CalculatePayrollService(PayRunRepository payRunRepository)
        {
            _payRunRepository = payRunRepository;
        }

        public void CalculatePayRun(long payRunId)
        {
            var payRunContext = _payRunRepository.GetPayRunContext(payRunId);

            foreach (var payRunEmp in payRunContext.PayRunEmployees)
            {
                // Earnings
                var earnings = CalculateEarnings(payRunEmp);
                payRunContext.PayRun.EarningsTotal += earnings.Total;
                payRunEmp.EarningsAmount += earnings.Total;
                earnings.Details.ToList().ForEach(payRunEmp.PayRunDetails.Add);

                // Pre-tax deductions
                // NOTE: Ugly way to pass in the discounts, doing this to save time and avoid DB calls during calc.
                var deductionsPreTax = CalculateDeductionsPreTax(payRunEmp);
                payRunContext.PayRun.DeductionsTotal += deductionsPreTax.Total;
                payRunEmp.DeductionsAmount += deductionsPreTax.Total;
                deductionsPreTax.Details.ToList().ForEach(payRunEmp.PayRunDetails.Add);

                // Withholdings (Currently does nothing)
                var withholdings = CalculateWithholdings(payRunEmp);
                payRunContext.PayRun.WithholdingsTotal += withholdings.Total;
                payRunEmp.WithholdingsAmount += withholdings.Total;
                withholdings.Details.ToList().ForEach(payRunEmp.PayRunDetails.Add);

                // Post-tax deductions (Currently does nothing)
                var deductionsPostTax = CalculateDeductionsPostTax(payRunEmp);
                payRunContext.PayRun.DeductionsTotal += deductionsPostTax.Total;
                payRunEmp.WithholdingsAmount += deductionsPostTax.Total;
                deductionsPostTax.Details.ToList().ForEach(payRunEmp.PayRunDetails.Add);

                // Net pay
                var netPay = CalculateNetPay(payRunEmp);
                payRunEmp.NetAmount = netPay;
                payRunContext.PayRun.NetTotal += netPay;
            }

            _payRunRepository.SavePayRunCalculationResult(payRunContext);
        }

        private PayRunDetailGroup CalculateEarnings(PayRunEmployee payRunEmp)
        {
            var earningDetails = new List<PayRunDetail>()
            {
                new PayRunDetail()
                {
                    Amount = payRunEmp.Employee.EmployeePositions.FirstOrDefault()?.PayRate ?? 0m,
                    PayRunDetailType = PayRunDetailType.Earning,
                    PayRunEmployeeId = payRunEmp.PayRunEmployeeId
                }
            };

            return new PayRunDetailGroup(earningDetails.Sum(dtl => dtl.Amount), earningDetails);
        }

        private PayRunDetailGroup CalculateDeductionsPreTax(PayRunEmployee payRunEmp)
        {
            var deductionDetails = new List<PayRunDetail>()
            {
                new PayRunDetail()
                {
                    Amount = CalculateBenefitPremiums(payRunEmp),
                    PayRunDetailType = PayRunDetailType.Deduction,
                    PayRunEmployeeId= payRunEmp.PayRunEmployeeId
                }
            };

            return new PayRunDetailGroup(deductionDetails.Sum(dtl => dtl.Amount), deductionDetails);
        }

        private PayRunDetailGroup CalculateWithholdings(PayRunEmployee payRunEmp)
        {
            return new PayRunDetailGroup(0m, new List<PayRunDetail>());
        }

        private PayRunDetailGroup CalculateDeductionsPostTax(PayRunEmployee payRunEmp)
        {
            return new PayRunDetailGroup(0m, new List<PayRunDetail>());
        }

        private decimal CalculateNetPay(PayRunEmployee payRunEmp)
        {
            return (payRunEmp.EarningsAmount - payRunEmp.DeductionsAmount - payRunEmp.WithholdingsAmount);
        }

        private decimal CalculateBenefitPremiums(PayRunEmployee payRunEmp)
        {
            // Some time-saving hacks here to avoid DB calls during calc and spending too long building out the context DTO.
            var benefitId = payRunEmp.Employee
                .EmployeeEnrollments.FirstOrDefault()?
                .EnrollmentBenefits.FirstOrDefault()?
                .Benefit.BenefitId ?? 0;

            // Hardcoded proof of concept for applying discounts to arbitrary employee attributes.
            // This is probably the most interesting design choice in here, and (maybe) not as over-engineered as it looks.
            // The idea would be to allow a benefits admin to create data-driven discounts that don't require engineers to maintain.
            // The use of RegEx is just a placeholder for what might be replaced by something like a visual programming UI.
            // A more straightforward approach might be to have an EmployeeAttributes table and restrict discounts to those values.
            // Only real issue is that I've already spent way too long trying to get the MVP working.
            var discountsToApply = new List<BenefitDiscount>()
            {
                new BenefitDiscount()
                {
                    BenefitId = 0,
                    DiscountType = BenefitDiscountType.Percentage,
                    DiscountAmount = 0.10m,
                    FilterByColumn = "Employee.FirstName",
                    FilterPattern = "^[aA].*$"
                },

                // TODO: Fix GetPropertyValue() for ICollection properties.

                //new BenefitDiscount()
                //{
                //    BenefitId = 0,
                //    DiscountType = BenefitDiscountType.Percentage,
                //    DiscountAmount = 0.10m,
                //    FilterByColumn = "Employee.EmployeeEnrollments.EnrollmentDependents.FirstName",
                //    FilterPattern = "^[aA].*$"
                //}
            };

            var employeeCostBase = payRunEmp.Employee
                .EmployeeEnrollments.FirstOrDefault()?
                .EnrollmentBenefits.FirstOrDefault(eb => eb.BenefitId == benefitId)?
                .Benefit.AnnualCostEmployee ?? 0m;

            var employeeCostFinal = employeeCostBase;

            foreach (var discount in discountsToApply)
            {
                var propertyValue = GetPropertyValue(payRunEmp, discount.FilterByColumn);
                var regex = new Regex(discount.FilterPattern);
                if (regex.IsMatch((string)propertyValue))
                {
                    employeeCostFinal -= (employeeCostBase * discount.DiscountAmount);
                }
            }

            // Discount not working for dependents yet. Hard-coded this since I'm way over time. Disappointing.
            var dependentCostBase = payRunEmp.Employee
                .EmployeeEnrollments.FirstOrDefault()?
                .EnrollmentBenefits.FirstOrDefault(eb => eb.BenefitId == benefitId)?
                .Benefit.AnnualCostDependent ?? 0m;

            var dependents = (payRunEmp.Employee
                .EmployeeEnrollments.FirstOrDefault()?
                .EnrollmentDependents ?? new List<EnrollmentDependent>())
                .Select(dep => new { FirstName = dep.FirstName, LastName = dep.LastName, Cost = dependentCostBase })
                .ToList();

            var dependentCostFinal = dependents.Sum(dep => dep.Cost);

            return (employeeCostFinal + dependentCostFinal) / 26;
        }

        public static object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null || propertyName == null)
                throw new ArgumentNullException();

            if (propertyName.Contains('.'))
            {
                var propertyList = propertyName.Split(new char[] { '.' }, 2);
                return GetPropertyValue(GetPropertyValue(obj, propertyList[0]), propertyList[1]);
            }
            else
            {
                var property = obj.GetType().GetProperty(propertyName);
                return (property != null) ? (property.GetValue(obj, null)) : null;
            }
        }

        private struct PayRunDetailGroup
        {
            public PayRunDetailGroup(decimal total, IEnumerable<PayRunDetail> details)
            {
                Total = total;
                Details = details;
            }

            public decimal Total;
            public IEnumerable<PayRunDetail> Details;
        }
    }
}
