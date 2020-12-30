using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stize.ApiTemplate.Api.Host.Configuration.OAuth2;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OAuth2ServiceCollectionExtensions
    {
        public static void AddOAuth2(this IServiceCollection services)
        {
            services.AddOptions<OAuth2Options>()
                .Configure<IConfiguration>((settings, config) =>
                    {
                        config.GetSection(OAuth2Options.Section).Bind(settings);
                    });

            services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerOptionsConfigure>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();
            
        }
    }
}