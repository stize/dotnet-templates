using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stize.ApiTemplate.Business.Models.ToDoItem;
using Stize.ApiTemplate.Business.Models.ToDoList;
using Stize.ApiTemplate.Business.Services;
using Stize.ApiTemplate.Domain.EFCore;
using Stize.DotNet.Search.Specification;
using Stize.Persistence.Materializer;

namespace Stize.ApiTemplate.Api.Extensions
{
    public static class StartupExtensions
    {
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public static IServiceCollection AddApi(this IServiceCollection services, Action<DbContextOptionsBuilder> configureEntityDbContext)
        { ;
            services.AddHttpContextAccessor();
            services.AddControllers()
                .AddApplicationPart(typeof(StartupExtensions).Assembly)
                .AddFileModel()
                .AddPageDescriptor()
                .AddDelta()
                ;

            services.AddApiAuthorization();

            services.AddStizeAspNetCore();
            services.AddStizePersistence();
            services.AddStizeMediator();
            services.AddScoped<ISpecificationBuilder, SpecificationBuilder>();

            services.AddStizeEntityDbContext<EntityDbContext>(configureEntityDbContext);
            services.AddStizeEntityRepository();
            services.AddStizeAutoMapper(typeof(ToDoListModel).Assembly);
            services.AddStizeMapperService();
            services.AddStizeCqrsEntityFrameworkCore();

            //TODO: Materializers registration
            services.AddScoped<IMaterializer<Domain.Entities.ToDoList, ToDoListModel>, ToDoListModelMaterializer>();
            services.AddScoped<IEntityBuilder<Domain.Entities.ToDoList, int>, ToDoListModelEntityBuilder>();
            services.AddScoped<IMaterializer<Domain.Entities.ToDoItem, ToDoItemModel>, ToDoItemModelMaterializer>();
            services.AddScoped<IToDoListService, ToDoListService>();
            services.AddScoped<ITodoItemService, TodoItemService>();
            

            return services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static IApplicationBuilder UseApi(this IApplicationBuilder app)
        {
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            return app;
        }
    }
}
