using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class UserWithTodoItemsResponse
    {


        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public List<ToDoItemResponse> TodoItems { get; set; }
    }
}
