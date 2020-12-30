using Stize.Domain.Entity;

namespace Stize.ApiTemplate.Domain.Entities
{
    public class ToDoItem : BaseEntity<int>
    {
        public bool IsCompleted { get; set; }

        public int ToDoListId { get; set; }
        public ToDoList ToDoList { get; set; }
        public string Name { get; set; }
    }
}