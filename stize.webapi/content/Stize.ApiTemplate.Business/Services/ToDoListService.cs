using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Stize.ApiTemplate.Business.Models.ToDoList;
using Stize.ApiTemplate.Domain.EFCore;
using Stize.ApiTemplate.Domain.Entities;
using Stize.DotNet.Result;
using Stize.DotNet.Search.Page;
using Stize.DotNet.Search.Specification;
using Stize.DotNet.Specification;
using Stize.Persistence;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Inquiry;
using Stize.Persistence.InquiryDispatcher;

namespace Stize.ApiTemplate.Business.Services
{
    public class ToDoListService : IToDoListService
    {
        private readonly IInquiryDispatcher dispatcher;
        private readonly IEntityRepository<EntityDbContext> entityRepository;
        private readonly ISpecificationBuilder specificationBuilder;

        public ToDoListService(IInquiryDispatcher dispatcher, IEntityRepository<EntityDbContext> entityRepository, ISpecificationBuilder specificationBuilder)
        {
            this.dispatcher = dispatcher;
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

                SourceQuery = this.entityRepository.GetAll<ToDoList>()
                
            };
            var result = await this.dispatcher.HandleAsync(query, cancellationToken);
            return result;
        }


    }
}
