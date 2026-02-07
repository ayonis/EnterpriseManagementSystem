using EnterpriseManagementSystem.Application.Interfaces;
using EnterpriseManagementSystem.Infrastructure.Data;
using EnterpriseManagementSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseManagementSystem.Infrastructure.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));


        services.AddScoped<IUnitOfWork, UnitOfWork>();


        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));



        return services;
    }
}

