using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stize.Testing.Xunit.AspNetCore;
using Stize.Testing.Xunit.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace Stize.ApiTemplate.Api.TestHost
{
    public class BaseTest : WebApplicationTest<Startup>
    {
        public BaseTest(WebApplicationFactory<Startup> fixture, ITestOutputHelper output) : base(fixture, output)
        {
           this.EnsureDatabaseCreated();
        }


        private void EnsureDatabaseCreated()
        {
            using (var scope = this.Fixture.Server.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DbContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }

        private void EnsureDatabaseDeleted()
        {
            using (var scope = this.Fixture.Server.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DbContext>();
                db.Database.EnsureDeleted();
            }
        }


    }
}