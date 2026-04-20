using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAppoinmentAPI.Models
{
    public class Doctor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DoctorId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required ,Range(0,50)]
        public int ExperienceYears { get; set; }

        [Required]
        public string Qualifications { get; set; }

        [Required, StringLength(800)]
        public string Summary { get; set; }

        [Required]
        public Specilization Specilization { get; set; }
        public int SpecilizationId {  get; set; }

        [Required]
        public string ProfileName { get; set; } 

        [Required]
        public HealthCareCenter HealthCareCenter { get; set; }
        public int HealthCareCenterId { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Availability> Availabilities { get; set; }
    }
}
