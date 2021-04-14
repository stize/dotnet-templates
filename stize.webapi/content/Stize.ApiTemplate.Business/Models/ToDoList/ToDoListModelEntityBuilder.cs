using Stize.ApiTemplate.Domain.EFCore;
using Stize.DotNet.Delta;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Materializer;
using System;
using System.Threading.Tasks;

namespace Stize.ApiTemplate.Business.Models.ToDoList
{
    public class ToDoListModelEntityBuilder : IEntityBuilder<Domain.Entities.ToDoList, int>
    {
        private readonly IEntityRepository<EntityDbContext> repository;

        public ToDoListModelEntityBuilder(IEntityRepository<EntityDbContext> repository)
        {
            this.repository = repository;
        }
       
       public Task<Domain.Entities.ToDoList> CreateAsync<TModel>(TModel model) where TModel : class
        {
            if (model is CreateToDoListModel m)
            {
                var entity = new Domain.Entities.ToDoList
                {
                    Name = m.Name
                };
                return Task.FromResult(entity);
            }

            throw new NotSupportedException();
        }

        public async Task<Domain.Entities.ToDoList> UpdateAsync<TModel>(TModel model) where TModel : class
        {
            if (model is UpdateToDoListModel m)
            {
                var entity = await this.repository.FindOneAsync<Domain.Entities.ToDoList, int>(m.Id);
                entity.Name = m.Name;
                return entity;
            }

            throw new NotSupportedException();
        }

        public async Task<Domain.Entities.ToDoList> PatchAsync<TModel>(Delta<TModel> model) where TModel : class
        {
            if (model is Delta<UpdateToDoListModel> d && 
                d.TryGetPropertyValue(nameof(UpdateToDoListModel.Id), out object idValue) && idValue is int id)
            {
                var entity = await this.repository.FindOneAsync<Domain.Entities.ToDoList, int>(id);
                var deltaEntity = new Delta<Domain.Entities.ToDoList>();
                foreach (var p in d.GetChangedPropertyNames())
                {
                    if(d.TryGetPropertyValue(p, out object v))
                    {
                        deltaEntity.TrySetPropertyValue(p, v);
                    }
                }

                deltaEntity.Patch(entity);

                return entity;
            }

            throw new NotSupportedException();
        }
    }
}