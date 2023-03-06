using Microsoft.EntityFrameworkCore;
using PaylocityPayrollApi.DataAccess.Model;
using PaylocityPayrollApi.Model;
using System.ComponentModel.Design;

namespace PaylocityPayrollApi.DataAccess.Repository
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

            payRun.EarningsTotal = 0m;
            payRun.DeductionsTotal = 0m;
            payRun.WithholdingsTotal = 0m;
            payRun.NetTotal = 0m;

            var payRunContext = new PayRunContextDto
            {
                PayRun = payRun,
                PayRunEmployees = _dbContext.Employees
                    .Include(e => e.EmployeePositions)
                    .Include(e => e.EmployeeEnrollments)
                        .ThenInclude(ee => ee.EnrollmentBenefits)
                        .ThenInclude(eb => eb.Benefit)
                    .Include(e => e.EmployeeEnrollments)
                        .ThenInclude(ee => ee.EnrollmentDependents)
                    .Where(e => e.CompanyId == payRun.CompanyId)
                    .Select(e => new PayRunEmployee()
                    {
                        PayRunId = payRun.PayRunId,
                        EmployeeId = e.EmployeeId,
                        Employee = e
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
