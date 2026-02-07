using EnterpriseManagementSystem.Application.Features.EmployeeFeature.Interfaces;
using EnterpriseManagementSystem.Application.Features.Feature1.Mappings;
using EnterpriseManagementSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseManagementSystem.Application.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(EmployeeMappingProfile).Assembly);

        // Register Application Services
        services.AddScoped<IEmployeeService, EmployeeService>();
       
        return services;
    }
}

