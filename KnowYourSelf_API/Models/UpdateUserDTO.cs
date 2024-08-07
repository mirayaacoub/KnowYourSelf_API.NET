using System.ComponentModel.DataAnnotations;

namespace KnowYourSelf_API.Models
{
    public class UpdateUserDTO
    {
        [Required]
        public int UserId { get; set; }
        public string? UserImageUrl { get; set; }
    }
}
