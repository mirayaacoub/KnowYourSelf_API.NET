using System.ComponentModel.DataAnnotations;

namespace KnowYourSelf_API.Models
{
    public class UpdatePatientDTO
    {
        [Required]
        public int UserId { get; set; }
        public string? DiagnosisHistory { get; set; } // Use nullable string
    }
}
