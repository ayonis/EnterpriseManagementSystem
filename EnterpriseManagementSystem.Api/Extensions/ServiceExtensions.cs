using EnterpriseManagementSystem.Api.Filters;
using EnterpriseManagementSystem.Api.Middlewares;
using EnterpriseManagementSystem.Api.Resources;
using Hangfire;
using Microsoft.Extensions.Options;

namespace EnterpriseManagementSystem.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddCustomLocalization(defaultCulture: "en", "en", "ar");

        services.AddControllers().AddDataAnnotationsLocalization(options =>
                                                                    {
                                                                        options.DataAnnotationLocalizerProvider = (type, factory) =>
                                                                            factory.Create(typeof(SharedResource));
                                                                    });

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services.AddAuthorization();

        return services;
    }


    public static WebApplication UseApiMiddlewarePipeline(this WebApplication app)
    {
        app.UseExceptionMiddleware();


        var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;

        app.UseRequestLocalization(localizationOptions);

        app.UseRouting();

        app.UseCors("AllowFrontend");

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();


        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new HangfireAuthorizationFilter() }
        });
        app.MapControllers();

        return app;
    }
}

