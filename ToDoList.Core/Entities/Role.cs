using System.ComponentModel.DataAnnotations;
using ToDoList.Core.Entities;

namespace Entities
{
    public class Role : BaseEntity
    {
      

        [Required]
        public string Name { get; set; }

        // Navigation property to Users
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}