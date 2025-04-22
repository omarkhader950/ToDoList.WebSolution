using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class TodoItemAddRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title can't be more than 100 characters")]
        public string? Title { get; set; }

        [MaxLength(500, ErrorMessage = "Description can't be more than 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        public DateTime? DueDate { get; set; }



        public Guid UserId { get; set; }

        /// <summary>
        /// Converts this AddRequest to a TodoItem entity.
        /// </summary>
        /// <returns></returns>
        public TodoItem ConvertToTodoItem()
        {
            return new TodoItem
            {
                Title = Title!,
                Description = Description,
                IsCompleted = false,
                DueDate = DueDate ?? DateTime.Now
            };
        }
    }
}