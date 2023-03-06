using PaylocityPayrollApi.DataAccess.Model;
using PaylocityPayrollApi.Enums;

namespace PaylocityPayrollApi.Model
{
    public class PayRunContextDto
    {
        public PayRunContextDto()
        {
            
        }

        public PayRun PayRun { get; set; } = new PayRun();
        public List<PayRunEmployee> PayRunEmployees { get; set; } = new List<PayRunEmployee>();
        public List<BenefitDiscount> BenefitDiscounts { get; set;}
    }
}
