using System.Threading;
using System.Threading.Tasks;
using Stize.DotNet.Search.Page;
using Stize.Persistence.QueryResult;

namespace Stize.ApiTemplate.Business.Services
{
    public interface ITodoItemService
    {
        Task CompleteAsync(int id, CancellationToken cancellationToken);
        
        Task<IMultipleQueryResult<T>> GetAllAsync<T>(int todoListId, CancellationToken cancellationToken) 
            where T : class;
       
        Task<IPagedQueryResult<T>> SearchAsync<T>(int todoListId, IPageDescriptor page, CancellationToken cancellationToken) 
            where T : class;
    }
}