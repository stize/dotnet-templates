using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stize.Domain.Entity;

namespace Stize.ApiTemplate.Domain.Entities
{
    public class ToDoList : BaseEntity<int>
    {
        public ICollection<ToDoItem> TodoItem { get; set; } = new List<ToDoItem>();

        public string Name { get; set; }
    }
}
