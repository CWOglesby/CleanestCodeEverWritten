using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace paylocity_payroll_api_test01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayRunController : ControllerBase
    {
        private readonly ILogger<PayRunController> _logger;

        public PayRunController(ILogger<PayRunController> logger)
        {
            _logger = logger;
        }
    }
}
