using System.Linq;
using Stize.Persistence.Materializer;

namespace Stize.ApiTemplate.Business.Models.ToDoList
{
    public class ToDoListModelMaterializer : IMaterializer<Domain.Entities.ToDoList, ToDoListModel>
    {
        public IQueryable<ToDoListModel> Materialize(IQueryable<Domain.Entities.ToDoList> queryable)
        {
            return queryable.Select(e => new ToDoListModel()
            {
                Id = e.Id,
                Name = e.Name
            });
        }
    }
}