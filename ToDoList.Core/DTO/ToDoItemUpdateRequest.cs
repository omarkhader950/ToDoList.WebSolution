using Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class ToDoItemUpdateRequest : ToDoDtoBase
    {
        [Required(ErrorMessage = "Task ID can't be blank")]
        public Guid Id { get; set; }

        public bool IsCompleted { get; set; }


        

        /// <summary>
        /// Converts the current TodoItemUpdateRequest into a TodoItem entity
        /// </summary>
        //public TodoItem ToTodoItem()
        //{
        //    var todoItem = this.Adapt<TodoItem>();  
        //    todoItem.IsCompleted = false; // Set defaults if needed
        //    return todoItem;
        //}


    }
}
