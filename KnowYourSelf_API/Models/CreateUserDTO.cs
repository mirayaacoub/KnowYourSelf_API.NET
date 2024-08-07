using System.ComponentModel.DataAnnotations;

namespace KnowYourSelf_API.Models
{
    public class CreateUserDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Role { get; set; }

        public bool IsVerified { get; set; }

        public string? UserImageUrl { get; set; }
    }
}
