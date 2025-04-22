using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class ToDoItemUpdateRequest
    {
        [Required(ErrorMessage = "Task ID can't be blank")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title can't be more than 100 characters")]
        public string? Title { get; set; }

        [MaxLength(500, ErrorMessage = "Description can't be more than 500 characters")]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        [Required(ErrorMessage = "Due Date is required")]
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Converts the current TodoItemUpdateRequest into a TodoItem entity
        /// </summary>
        public TodoItem ToTodoItem()
        {
            return new TodoItem
            {
                Id = Id,
                Title = Title!,
                Description = Description,
                IsCompleted = IsCompleted,
                DueDate = DueDate ?? DateTime.Now
            };
        }


    }
}
