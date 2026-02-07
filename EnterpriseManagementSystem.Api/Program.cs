using AuthenticationModule.Infrastructure.DependencyInjection;
using EnterpriseManagementSystem.Api.Extensions;
using EnterpriseManagementSystem.Application.DependencyInjection;
using EnterpriseManagementSystem.Infrastructure.DependencyInjection;

namespace EnterpriseManagementSystem.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Services.AddApiServices(configuration);
            builder.Services.AddHangfire(configuration);
            builder.Services.AddSecurityServices(configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(configuration);
            
            var app = builder.Build();

            
            app.UseApiMiddlewarePipeline();

            app.Run();
        }
    }
}
