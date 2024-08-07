using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KnowYourSelf_API.Models
{
    public class CreateBlogpostDTO
    {
        [Required]
        public int BlogpostId { get; set; }
        [Required]
        public string? BlogpostTitle { get; set; }
        [Required]
        public string? BlogpostContent { get; set; }
        [Required]
        public string? Category { get; set; } 
        public string? BlogpostImage { get; set; }
        [Required, NotNull]
        public int TherapistId { get; set; } // fk prop
        [Required]
        public DateTime CreatedAt { get; set; }

    }
}
