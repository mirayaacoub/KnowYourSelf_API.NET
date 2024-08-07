using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace KnowYourSelf_API.Models
{
    public class Blogpost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogpostId { get; set; }
        [Required]
        public string? BlogpostTitle { get; set; }
        [Required]
        public string? BlogpostContent { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
        public string? Category { get; set; } 
        public string? BlogpostImage { get; set; }
        [Required, NotNull]
        public int TherapistId { get; set; } // fk prop
        // navigation property
        [ForeignKey("TherapistId")]
        public virtual Therapist Therapist { get; set; }




    }
}
