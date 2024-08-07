using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace KnowYourSelf_API.Models
{
    public class Therapist
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TherapistId { get; set; }

        public int? ExperienceYears { get; set; } // Use nullable int
        public string? Specialty { get; set; } // Use nullable string
                               
        [Required, NotNull]
        public int UserId { get; set; } // fk prop
        // navigation property
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public virtual Blogpost? Blogpost { get; set; }

    }
}
