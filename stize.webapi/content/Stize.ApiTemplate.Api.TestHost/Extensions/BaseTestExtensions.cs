using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Stize.ApiTemplate.Api.TestHost.Extensions
{
    public static class BaseTestExtensions
    {
        public static async Task<T> WithEntityInTheDatabaseAsync<T>(this BaseTest current, T entity)
        {
            await current.ExecuteDbContextAsync(async context =>
            {
                await context.AddAsync(entity);
                await context.SaveChangesAsync();
            });
            return entity;
        }

        public static async Task ExecuteDbContextAsync(this BaseTest current, Func<DbContext, Task> action)
        {
            await current.ExecuteScopeAsync(sp => action(sp.GetService<DbContext>()));
        }

        public static async Task ExecuteScopeAsync(this BaseTest current, Func<IServiceProvider, Task> action)
        {
            using (var scope = current.Fixture.Server.Services.CreateScope())
            {
                await action(scope.ServiceProvider);
            }
        }
    }
}
