using System.ComponentModel.DataAnnotations;
using ToDoList.Core.Entities;

namespace Entities
{
    public class User : BaseEntity
    {

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
