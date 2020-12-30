using Stize.Domain.Model;

namespace Stize.ApiTemplate.Business.Models.ToDoItem
{
    public class CreateToDoItemModel: BaseModel
    {
        public int ToDoListId { get; set; }
        public string Name { get; set; }
    }
}