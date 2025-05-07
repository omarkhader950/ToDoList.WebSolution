using System.ComponentModel.DataAnnotations;

namespace Entities
{



    public enum TodoStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2,
        Cancelled = 3
    }



    public class TodoItem
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime DueDate { get; set; }

   

        // Foreign key to the User entity (one-to-many relationship)
        public Guid UserId { get; set; }

        // Navigation property for the related user
        public User User { get; set; }


        //creation date 
        public DateTime CreationDate { get; set; }

        //delete date 
        public DateTime? DeleteDate { get; set; }

        //delete by
        public Guid? DeleteBy {  get; set; }


        // Status using the enum
        public TodoStatus Status { get; set; } = TodoStatus.Pending;

    }
}
