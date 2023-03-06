using Microsoft.AspNetCore.Mvc;
using paylocity_payroll_api_test01.DataAccess.Model;

namespace paylocity_payroll_api_test01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BenefitController : ControllerBase
    {
        private readonly ILogger<BenefitController> _logger;

        public BenefitController(ILogger<BenefitController> logger)
        {
            _logger = logger;
        }
    }
}