using AuthenticationModule.Api.Extensions;
using AuthenticationModule.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthenticationModule.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Services.AddAuthenticationModuleInfrastructure(configuration);
        builder.Services.AddApiServices(configuration);
        builder.Services.AddSecurityServices(configuration);

        var app = builder.Build();

        app.UseApiMiddlewarePipeline();
       

        app.Run();
    }
}
