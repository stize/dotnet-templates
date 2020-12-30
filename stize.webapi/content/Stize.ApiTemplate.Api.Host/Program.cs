using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stize.ApiTemplate.Domain.EFCore;

namespace Stize.ApiTemplate.Api.Host
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<EntityDbContext>();
                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
