using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class TodoItemAddRequest : ToDoDtoBase
    {

        // Optional — only used by Admins
        public Guid? UserId { get; set; }

        ///// <summary>
        ///// Converts this AddRequest to a TodoItem entity.
        ///// </summary>
        ///// <returns></returns>
        //public TodoItem ConvertToTodoItem()
        //{
        //    var todoItem = this.Adapt<TodoItem>(); // Magic happens here
        //    todoItem.IsCompleted = false; // Set defaults if needed
        //    return todoItem;
        //}
    }
    }
