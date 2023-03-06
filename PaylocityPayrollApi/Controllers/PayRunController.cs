using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaylocityPayrollApi.Services.Payroll;

namespace PaylocityPayrollApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayRunController : ControllerBase
    {
        private readonly ILogger<PayRunController> _logger;
        private readonly CreatePayRunService _payRunService;
        private readonly CalculatePayrollService _payCalcService;

        public PayRunController(ILogger<PayRunController> logger, CreatePayRunService payRunService, CalculatePayrollService payCalcService)
        {
            _logger = logger;
            _payRunService = payRunService;
            _payCalcService = payCalcService;
        }

        [HttpPost("create")]
        public void CreatePayRun(long companyId, DateTime payPeriodFrom, DateTime payPeriodTo, DateTime payDate)
        {
            _payRunService.CreatePayRun(companyId, payPeriodFrom, payPeriodTo, payDate);
        }

        [HttpPost("calculate")]
        public void CalculatePayRun(long payRunId)
        {
            _payCalcService.CalculatePayRun(payRunId);
        }
    }
}
