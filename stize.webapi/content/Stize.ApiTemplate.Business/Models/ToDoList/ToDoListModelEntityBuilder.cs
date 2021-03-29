using Stize.DotNet.Delta;
using Stize.Persistence.Materializer;

namespace Stize.ApiTemplate.Business.Models.ToDoList
{
    public class ToDoListModelEntityBuilder : IEntityBuilder<ToDoListModel, Domain.Entities.ToDoList, int>
    {
        public Domain.Entities.ToDoList Create(ToDoListModel model)
        {
            throw new System.NotImplementedException();
        }

        public Domain.Entities.ToDoList Patch(Delta<ToDoListModel> model)
        {
            throw new System.NotImplementedException();
        }

        public Domain.Entities.ToDoList Update(ToDoListModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}