using System.Threading;
using System.Threading.Tasks;
using Stize.ApiTemplate.Business.Models.ToDoList;
using Stize.ApiTemplate.Domain.EFCore;
using Stize.ApiTemplate.Domain.Entities;
using Stize.DotNet.Result;
using Stize.DotNet.Search.Page;
using Stize.DotNet.Search.Specification;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Inquiry;

namespace Stize.ApiTemplate.Business.Services
{
    public class ToDoListService : IToDoListService
    {
        private readonly IEntityRepository<EntityDbContext> entityRepository;
        private readonly ISpecificationBuilder specificationBuilder;

        public ToDoListService(IEntityRepository<EntityDbContext> entityRepository, ISpecificationBuilder specificationBuilder)
        {
            this.entityRepository = entityRepository;
            this.specificationBuilder = specificationBuilder;
        }
        public async Task<PagedValueResult<ToDoListModel>> SearchAsync(IPageDescriptor page, CancellationToken cancellationToken = default)
        {
            var spec = this.specificationBuilder.Create<ToDoListModel>(page.Filters);
            var query = new PagedValueInquiry<ToDoList, ToDoListModel>() {                
                Take = page.Take,
                Skip = page.Skip,
                TargetSpecification = spec,
                TargetSorts = page.Sorts,                
            };
            var result = await this.entityRepository.RunQueryAsync(query, cancellationToken);
            return result;
        }


    }
}
