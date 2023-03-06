using Microsoft.EntityFrameworkCore;
using paylocity_payroll_api_test01.DataAccess;
using paylocity_payroll_api_test01.DataAccess.Repository;
using paylocity_payroll_api_test01.Services.Payroll;

namespace paylocity_payroll_api_test01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var payrollDbConnection = builder.Configuration["ConnectionStrings:PayrollDatabase"];
            builder.Services.AddDbContext<PayrollDbContext>(options =>
                options.UseSqlServer(payrollDbConnection));

            builder.Services.AddScoped<CreatePayRunService>();
            builder.Services.AddScoped<CalculatePayrollService>();
            builder.Services.AddScoped<PayRunRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}