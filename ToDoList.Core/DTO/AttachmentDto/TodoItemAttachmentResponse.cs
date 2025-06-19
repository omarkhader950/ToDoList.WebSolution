using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.DTO.AttachmentDto
{
    public class TodoItemAttachmentResponse
    {
        public Guid Id { get; set; }
        public Guid TodoItemId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public string? UploadedBy { get; set; }
    }
}