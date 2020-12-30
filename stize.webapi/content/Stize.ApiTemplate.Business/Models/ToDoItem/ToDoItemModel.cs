using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stize.ApiTemplate.Business.Models.ToDoList;
using Stize.Domain.Model;

namespace Stize.ApiTemplate.Business.Models.ToDoItem
{
    public class ToDoItemModel: BaseModel<int>
    {
        public int ToDoListId { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
    }
}
