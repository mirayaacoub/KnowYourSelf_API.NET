using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KnowYourSelf_API.Models
{
    public class UpdateBlogpostDTO
    {
        [Required]
        public int BlogpostId { get; set; }
        [Required]
        public string? BlogpostTitle { get; set; }
        [Required]
        public string? BlogpostContent { get; set; }
        [Required]
        public string? BlogpostImage { get; set;  }
        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
