using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace KnowYourSelf_API.Models
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated (DatabaseGeneratedOption.Identity)]  
        public int ScheduleId { get; set; }
        public bool IsBooked { get; set; } = false;
        [Required]
        public  DateTime ScheduleDateTime { get; set; }
        [Required]
        public int TherapistId { get; set; } //fk required
        public int PatientId { get; set; } // fk not required unless is booked
    }
}
