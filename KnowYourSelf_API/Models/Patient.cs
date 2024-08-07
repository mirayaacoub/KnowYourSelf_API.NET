using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace KnowYourSelf_API.Models
{
    public class Patient
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PatientId { get; set; }
        public string? DiagnosisHistory { get; set; }

        [Required, NotNull]
        public int UserId { get; set; } // fk prop
        // navigation property
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
