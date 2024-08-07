using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace KnowYourSelf_API.Models
{
    public class User
    {
        public User() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required,NotNull]
        public string Email { get; set; }
        [Required, NotNull]
        public string Password { get; set; }
        [Required, NotNull]
        public string Username { get; set; }
        [Required, NotNull]
        [RoleValidation]
        public string Role { get; set; } = "Patient";
        public bool IsVerified  { get; set; }
        public string? UserImageUrl { get; set; }
        public virtual Therapist? Therapist { get; set; }
        public virtual Patient? Patient { get; set; }

        public class RoleValidationAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var role = value?.ToString();
                if (string.IsNullOrEmpty(role) || !IsValidRole(role))
                {
                    return new ValidationResult("Invalid role. Allowed values are: Admin, Therapist, Patient.");
                }

                return ValidationResult.Success;
            }

            private bool IsValidRole(string role)
            {
                return role == UserRole.Admin.ToString() || role == UserRole.Therapist.ToString() || role == UserRole.Patient.ToString();
            }
        }

    }

    
}
