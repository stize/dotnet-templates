using Stize.DotNet.Specification;

namespace Stize.ApiTemplate.Domain.Specifications.ToDoItem
{
    public class ToDoItemByIdSpecification : Specification<Entities.ToDoItem>
    {
        public ToDoItemByIdSpecification(int todoListId, int id) : 
            base(x => x.ToDoListId == todoListId && x.Id == id)
        {
        }
    }
}