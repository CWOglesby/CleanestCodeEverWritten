using paylocity_payroll_api_test01.DataAccess;
using paylocity_payroll_api_test01.DataAccess.Repository;

namespace paylocity_payroll_api_test01.Services.Payroll
{
    public class CreatePayRunService
    {
        private readonly PayRunRepository _payRunRepository;

        public CreatePayRunService(PayRunRepository payRunRepository)
        {
            _payRunRepository = payRunRepository;
        }

        public void CreatePayRun(long companyId, DateTime payPeriodFrom, DateTime payPeriodTo, DateTime payDate)
        {
            _payRunRepository.CreatePayRun(companyId, payPeriodFrom, payPeriodTo, payDate);
        }
    }
}
