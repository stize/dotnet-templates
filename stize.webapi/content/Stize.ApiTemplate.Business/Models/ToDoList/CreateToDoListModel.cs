using Stize.Domain.Model;

namespace Stize.ApiTemplate.Business.Models.ToDoList
{
    public class CreateToDoListModel : BaseModel
    {
        public string Name { get; set; }
    }
}