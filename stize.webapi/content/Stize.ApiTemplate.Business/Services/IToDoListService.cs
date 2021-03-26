using System.Threading;
using System.Threading.Tasks;
using Stize.ApiTemplate.Business.Models.ToDoList;
using Stize.DotNet.Result;
using Stize.DotNet.Search.Page;

namespace Stize.ApiTemplate.Business.Services
{
    public interface IToDoListService
    {
        Task<PagedValueResult<ToDoListModel>> SearchAsync(IPageDescriptor page, CancellationToken cancellationToken = default);
    }
}