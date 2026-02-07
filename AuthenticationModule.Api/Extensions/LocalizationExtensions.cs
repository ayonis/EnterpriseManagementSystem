using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace AuthenticationModule.Api.Extensions
{
    public static class LocalizationExtensions
    {
        public static IServiceCollection AddCustomLocalization(this IServiceCollection services,string defaultCulture, params string[] supportedCultures)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultureInfos = supportedCultures
                    .Select(c => new CultureInfo(c))
                    .ToArray();

                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = cultureInfos;
                options.SupportedUICultures = cultureInfos;
                options.ApplyCurrentCultureToResponseHeaders = true;

                options.RequestCultureProviders.Clear();
                options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider());
                options.RequestCultureProviders.Add(new CookieRequestCultureProvider());
                options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
            });

            return services;
        }
    }
}
