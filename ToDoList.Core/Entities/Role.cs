using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Navigation property to Users
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}