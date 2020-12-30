using Stize.Domain.Model;

namespace Stize.ApiTemplate.Business.Models.ToDoList
{
    public class UpdateToDoListModel : BaseModel<int>
    {
        public string Name { get; set; }
    }
}