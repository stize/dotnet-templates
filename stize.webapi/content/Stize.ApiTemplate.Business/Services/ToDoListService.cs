using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Stize.ApiTemplate.Business.Models.ToDoList;
using Stize.ApiTemplate.Domain.Entities;
using Stize.DotNet.Search.Page;
using Stize.DotNet.Search.Specification;
using Stize.DotNet.Specification;
using Stize.Persistence;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.ApiTemplate.Business.Services
{
    public class ToDoListService : IToDoListService
    {
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ISpecificationBuilder specificationBuilder;

        public ToDoListService(IQueryDispatcher queryDispatcher, ISpecificationBuilder specificationBuilder)
        {
            this.queryDispatcher = queryDispatcher;
            this.specificationBuilder = specificationBuilder;
        }
        public async Task<IPagedQueryResult<ToDoListModel>> SearchAsync(IPageDescriptor page, CancellationToken cancellationToken = default)
        {
            var spec = this.specificationBuilder.Create<ToDoList>(page.Filters);
            var query = new PagedQuery<ToDoList>(spec, page.Take, page.Skip) {
                Sorts = page.Sorts
            };
            var result = await this.queryDispatcher.DispatchAsync<PagedQuery<ToDoList>, PagedQueryResult<ToDoListModel>>(query, cancellationToken);
            return result;
        }


    }
}
