using System.ComponentModel.DataAnnotations;

namespace BookStore.DataBaseEntities
{

    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

    }
}