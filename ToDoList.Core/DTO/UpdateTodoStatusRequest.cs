using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.DTO
{
    public class UpdateTodoStatusRequest
    {
        [Required]
        public List<Guid> TodoItemIds { get; set; }
    }
}
