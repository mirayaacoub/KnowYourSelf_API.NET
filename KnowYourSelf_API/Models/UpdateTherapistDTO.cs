using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KnowYourSelf_API.Models
{
    public class UpdateTherapistDTO
    {
        [Required]
        public int UserId { get; set; }
        public int? ExperienceYears { get; set; } // Use nullable int
        public string? Specialty { get; set; } // Use nullable string
    }
}
