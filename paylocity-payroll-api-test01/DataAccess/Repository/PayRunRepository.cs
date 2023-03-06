using paylocity_payroll_api_test01.DataAccess.Model;
using paylocity_payroll_api_test01.Model;
using System.ComponentModel.Design;

namespace paylocity_payroll_api_test01.DataAccess.Repository
{
    public class PayRunRepository
    {
        private readonly PayrollDbContext _dbContext;

        public PayRunRepository(PayrollDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreatePayRun(long companyId, DateTime payPeriodFrom, DateTime payPeriodTo, DateTime payDate)
        {
            var payRun = new PayRun()
            {
                CompanyId = companyId,
                PayPeriodFrom = payPeriodFrom.Date,
                PayPeriodTo = payPeriodTo.Date,
                PayDate = payDate.Date
            };

            _dbContext.PayRuns.Add(payRun);
            _dbContext.SaveChanges();
        }

        public PayRun GetPayRunById(long payRunId)
        {
            return _dbContext.PayRuns.SingleOrDefault(pr => pr.PayRunId == payRunId);
        }

        public IEnumerable<PayRun> GetPayRunsByCompanyId(long companyId)
        {
            return _dbContext.PayRuns.Where(pr => pr.CompanyId == companyId).ToList();
        }

        public PayRunContextDto GetPayRunContext(long payRunId)
        {
            var payRun = _dbContext.PayRuns.SingleOrDefault(pr => pr.PayRunId == payRunId);

            if (payRun == null)
                return new PayRunContextDto();

            var payRunContext = new PayRunContextDto
            {
                PayRun = new PayRun()
                {
                    PayRunId = payRun.PayRunId,
                    CompanyId = payRun.CompanyId,
                    PayPeriodFrom = payRun.PayPeriodFrom,
                    PayPeriodTo = payRun.PayPeriodTo,
                    PayDate = payRun.PayDate,
                    PayRunType = payRun.PayRunType,
                    PayRunStatus = Enums.PayRunStatus.Calculating
                },

                PayRunEmployees = _dbContext.Employees
                    .Where(ee => ee.CompanyId == payRun.CompanyId)
                    .Select(ee => new PayRunEmployee()
                    {
                        PayRunId = payRun.PayRunId,
                        EmployeeId = ee.EmployeeId
                    }).ToList()
            };

            return payRunContext;
        }

        public void SavePayRunCalculationResult(PayRunContextDto payRunContext)
        {
            _dbContext.PayRuns.Update(payRunContext.PayRun);
            _dbContext.AddRange(payRunContext.PayRunEmployees);
            _dbContext.AddRange(payRunContext.PayRunEmployees.SelectMany(pre => pre.PayRunDetails));
            _dbContext.SaveChanges();
        }
    }
}
