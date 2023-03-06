using PaylocityPayrollApi.DataAccess;
using PaylocityPayrollApi.DataAccess.Repository;

namespace PaylocityPayrollApi.Services.Payroll
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
