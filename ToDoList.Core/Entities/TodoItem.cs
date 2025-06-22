using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDoList.Core.Entities;
using ToDoList.Core.Enums;
using ToDoList.Core.ServiceContracts;

namespace Entities
{






    public class TodoItem : BaseEntity , IOwnedResource
    {
        

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        //
        public DateTime DueDate { get; set; }

   

        // Foreign key to the User entity (one-to-many relationship)
        public Guid UserId { get; set; }

        // Navigation property for the related user
        public User User { get; set; }


        //creation date 
        public DateTime CreationDate { get; set; }

        public DateTime? DeleteDate { get; set; }

       
        public Guid? DeleteBy {  get; set; }


        public TodoStatus Status { get; set; } = TodoStatus.New;

        [NotMapped]
        // Map UserId to OwnerId
        public Guid OwnerId => UserId;


        public ICollection<TodoItemAttachment> Attachments { get; set; } = new List<TodoItemAttachment>();


    }
}
