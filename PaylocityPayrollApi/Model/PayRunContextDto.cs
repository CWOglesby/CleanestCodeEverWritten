using paylocity_payroll_api_test01.DataAccess.Model;
using paylocity_payroll_api_test01.Enums;

namespace paylocity_payroll_api_test01.Model
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
