using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stize.ApiTemplate.Api.Extensions;

namespace Stize.ApiTemplate.Api.TestHost
{
    public class Startup
    {
        private IWebHostEnvironment environment;
        private IConfiguration configuration;

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            this.environment = env;
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApi(opt =>
            {
                opt.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddTestServerAuthentication();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseApi();
        }
    }
}