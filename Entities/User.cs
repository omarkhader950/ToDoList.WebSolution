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

        public string Role { get; set; } = "User"; // Optional: for role-based authorization

        // Navigation property to represent the list of To-Do items created by the user
        public List<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
    }
}
