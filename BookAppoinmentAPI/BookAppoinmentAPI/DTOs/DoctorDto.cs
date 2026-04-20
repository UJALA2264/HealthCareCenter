using System.ComponentModel.DataAnnotations;

namespace BookAppoinmentAPI.DTOs
{
    public class DoctorDto
    {
        public string FullName { get; set; }

        public int ExperienceYears { get; set; }

        public string Qualifications { get; set; }

        public string Summary { get; set; }

        public int SpecilizationId { get; set; }

        public int HealthCareCenterId { get; set; }

        public IFormFile ProfileImage { get; set; }
    }
}
