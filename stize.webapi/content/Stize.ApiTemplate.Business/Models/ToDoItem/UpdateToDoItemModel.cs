using Stize.Domain.Model;

namespace Stize.ApiTemplate.Business.Models.ToDoItem
{
    public class UpdateToDoItemModel: BaseModel<int>
    {
        public string Name { get; set; }
    }
}