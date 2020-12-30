using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stize.ApiTemplate.Api.Configuration.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiAuthorizationServiceCollectionExtensions
    {
        public static void AddApiAuthorization(this IServiceCollection services)
        {
            services.AddOptions<ApiAuthorizationOptions>()
                .Configure<IConfiguration>((settings, config) =>
                    {
                        config.GetSection(ApiAuthorizationOptions.Section).Bind(settings);
                    });

            services.AddAuthorization();
            services.AddSingleton<IConfigureOptions<AuthorizationOptions>, AuthorizationOptionsConfigure>();

        }


    }
}