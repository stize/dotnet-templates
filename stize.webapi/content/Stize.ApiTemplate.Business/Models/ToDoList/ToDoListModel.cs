using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stize.Domain.Model;

namespace Stize.ApiTemplate.Business.Models.ToDoList
{
    public class ToDoListModel : BaseModel<int>
    {
        public string Name { get; set; }
    }
}
