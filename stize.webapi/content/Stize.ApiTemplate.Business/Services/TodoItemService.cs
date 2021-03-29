using System.Threading;
using System.Threading.Tasks;
using Stize.ApiTemplate.Domain.EFCore;
using Stize.ApiTemplate.Domain.Entities;
using Stize.DotNet.Result;
using Stize.DotNet.Search.Page;
using Stize.DotNet.Search.Specification;
using Stize.DotNet.Specification;
using Stize.DotNet.Specification.Extensions;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Inquiry;

namespace Stize.ApiTemplate.Business.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly IEntityRepository<EntityDbContext> repository;
        private readonly ISpecificationBuilder specificationBuilder;

        public TodoItemService(IEntityRepository<EntityDbContext> repository, ISpecificationBuilder specificationBuilder)
        {
            this.repository = repository;
            this.specificationBuilder = specificationBuilder;
        }

        public async Task CompleteAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await this.repository.FindOneAsync<ToDoItem, int>(id, cancellationToken);
            entity.IsCompleted = true;
            await this.repository.CommitAsync(cancellationToken);
        }

        public async Task<MultipleValueResult<T>> GetAllAsync<T>(int todoListId, CancellationToken cancellationToken)
            where T : class
        {
            var spec = new Specification<ToDoItem>(x => x.ToDoListId == todoListId);

            var query = new MultipleValueInquiry<ToDoItem, T>()
            {
                SourceSpecification = spec,
            };
            var result = await this.repository.RunQueryAsync(query, cancellationToken);
            return result;
        }

        public async Task<PagedValueResult<T>> SearchAsync<T>(int todoListId, IPageDescriptor page, CancellationToken cancellationToken) where T : class
        {
            var spec = this.specificationBuilder.Create<ToDoItem>(page.Filters);
            spec = spec.And(new Specification<ToDoItem>(x => x.ToDoListId == todoListId));

            var query = new PagedValueInquiry<ToDoItem, T>()
            {
                Take = page.Take,
                Skip = page.Skip,
                SourceSpecification = spec,
                SourceSorts = page.Sorts,
            };
            var result = await this.repository.RunQueryAsync(query, cancellationToken);
            return result;
        }
    }
}
