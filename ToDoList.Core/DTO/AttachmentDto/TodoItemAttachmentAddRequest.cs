using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.DTO.AttachmentDto
{
    public class TodoItemAttachmentAddRequest
    {
        public Guid TodoItemId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string? UploadedBy { get; set; }
    }
}