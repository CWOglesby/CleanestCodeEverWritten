using Microsoft.AspNetCore.Mvc;
using PaylocityPayrollApi.DataAccess.Model;

namespace PaylocityPayrollApi.Controllers
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