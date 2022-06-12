using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOModels
{

    public class UserRegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [MaxLength(25)]
        public string FirstName { get; set; }
        [MaxLength(25)]
        public string LastName { get; set; }
        public string Address { get; set; }
        public int RoleId { get; set; } = 1;
    }
}