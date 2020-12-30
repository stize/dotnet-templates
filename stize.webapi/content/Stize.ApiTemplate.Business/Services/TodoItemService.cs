using System.Threading;
using System.Threading.Tasks;
using Stize.ApiTemplate.Domain.EFCore;
using Stize.ApiTemplate.Domain.Entities;
using Stize.DotNet.Search.Page;
using Stize.DotNet.Search.Specification;
using Stize.DotNet.Specification;
using Stize.DotNet.Specification.Extensions;
using Stize.Persistence;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.ApiTemplate.Business.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly IEntityRepository<EntityDbContext> repository;
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ISpecificationBuilder specificationBuilder;

        public TodoItemService(IEntityRepository<EntityDbContext> repository, IQueryDispatcher queryDispatcher, ISpecificationBuilder specificationBuilder)
        {
            this.repository = repository;
            this.queryDispatcher = queryDispatcher;
            this.specificationBuilder = specificationBuilder;
        }
        
        public async Task CompleteAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await this.repository.FindOneAsync<ToDoItem, int>(id, cancellationToken);
            entity.IsCompleted = true;
            await this.repository.CommitAsync(cancellationToken);
        }

        public async Task<IMultipleQueryResult<T>> GetAllAsync<T>(int todoListId, CancellationToken cancellationToken)
            where T : class
        {
            var spec = new Specification<ToDoItem>(x => x.ToDoListId == todoListId);

            var query = new Query<ToDoItem>(spec);
            var result = await this.queryDispatcher.DispatchAsync<Query<ToDoItem>, MultipleQueryResult<T>>(query, cancellationToken);
            return result;  
        }

        public async Task<IPagedQueryResult<T>> SearchAsync<T>(int todoListId, IPageDescriptor page, CancellationToken cancellationToken) where T : class
        {
            var spec = this.specificationBuilder.Create<ToDoItem>(page.Filters);
            spec = spec.And(new Specification<ToDoItem>(x => x.ToDoListId == todoListId));

            var query = new PagedQuery<ToDoItem>(spec, page.Take, page.Skip) {
                Sorts = page.Sorts
            };
            var result = await this.queryDispatcher.DispatchAsync<PagedQuery<ToDoItem>, PagedQueryResult<T>>(query, cancellationToken);
            return result;  
        }
    }
}
