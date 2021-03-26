using System.Threading;
using System.Threading.Tasks;
using Stize.DotNet.Result;
using Stize.DotNet.Search.Page;

namespace Stize.ApiTemplate.Business.Services
{
    public interface ITodoItemService
    {
        Task CompleteAsync(int id, CancellationToken cancellationToken);
        
        Task<MultipleValueResult<T>> GetAllAsync<T>(int todoListId, CancellationToken cancellationToken) 
            where T : class;
       
        Task<PagedValueResult<T>> SearchAsync<T>(int todoListId, IPageDescriptor page, CancellationToken cancellationToken) 
            where T : class;
    }
}