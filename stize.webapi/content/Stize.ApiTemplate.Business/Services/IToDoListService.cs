using System.Threading;
using System.Threading.Tasks;
using Stize.ApiTemplate.Business.Models.ToDoList;
using Stize.DotNet.Search.Page;
using Stize.Persistence.QueryResult;

namespace Stize.ApiTemplate.Business.Services
{
    public interface IToDoListService
    {
        Task<IPagedQueryResult<ToDoListModel>> SearchAsync(IPageDescriptor page, CancellationToken cancellationToken = default);
    }
}