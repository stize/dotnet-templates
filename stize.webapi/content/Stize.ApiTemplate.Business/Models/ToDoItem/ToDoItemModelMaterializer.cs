using System.Linq;
using Stize.Persistence.Materializer;

namespace Stize.ApiTemplate.Business.Models.ToDoItem
{
    public class ToDoItemModelMaterializer : IMaterializer<Domain.Entities.ToDoItem, ToDoItemModel>
    {
        public IQueryable<ToDoItemModel> Materialize(IQueryable<Domain.Entities.ToDoItem> queryable)
        {
            return queryable.Select(e => new ToDoItemModel()
            {
                Id = e.Id,
                ToDoListId = e.ToDoListId,
                Name = e.Name,
                IsCompleted = e.IsCompleted
            });
        }
    }
}