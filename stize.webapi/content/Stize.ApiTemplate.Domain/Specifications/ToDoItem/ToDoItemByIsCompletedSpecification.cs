using Stize.DotNet.Specification;

namespace Stize.ApiTemplate.Domain.Specifications.ToDoItem
{
    public class ToDoItemByIsCompletedSpecification : Specification<Entities.ToDoItem>
    {
        public ToDoItemByIsCompletedSpecification(bool isCompleted) : 
            base(x => x.IsCompleted == isCompleted)
        {
        }
    }
}
