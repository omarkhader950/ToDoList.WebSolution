using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDoList.Core.Entities;
using ToDoList.Core.Enums;
using ToDoList.Core.ServiceContracts;

namespace Entities
{


    public class TodoItemAttachment : BaseEntity
    {
        [Required]
        public Guid TodoItemId { get; set; }

        [ForeignKey(nameof(TodoItemId))]
        public TodoItem TodoItem { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; }

        [Required]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public string UploadedBy { get; set; }



    }
}
