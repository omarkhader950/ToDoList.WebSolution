using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }


        // Foreign key to Role
        public Guid RoleId { get; set; }


       



        // Navigation property
        public Role Role { get; set; }


        // Navigation property to represent the list of To-Do items created by the user
        public List<TodoItem> TodoItems { get; set; } = new List<TodoItem>();


        public ICollection<Role> Roles { get; set; } = new List<Role>();

    }
}
