using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class ToDoItemResponse
    {

       
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime DueDate { get; set; }

        public string UserName { get; set; }




    }



    public static class TodoItemExtensions
    {
        /// <summary>
        /// Converts a TodoItem entity to a TodoItemResponse DTO
        /// </summary>
        public static ToDoItemResponse ToTodoItemResponse(this TodoItem todo)
        {
            return new ToDoItemResponse
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                DueDate = todo.DueDate,
                UserName = todo.User.Username,
                
            };
        }
    }
}
