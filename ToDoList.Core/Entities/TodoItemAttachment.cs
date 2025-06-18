using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.Entities
{
    using global::Entities;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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

}
