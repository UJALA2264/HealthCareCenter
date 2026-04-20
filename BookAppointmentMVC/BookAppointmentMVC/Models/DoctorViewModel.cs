using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookAppointmentMVC.Models
{
    public class DoctorViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required, Range(0, 50)]
        public int ExperienceYears { get; set; }

        [Required]
        public string Qualifications { get; set; }

        [Required, StringLength(800)]
        public string Summary { get; set; }

        [Required]
        public int SpecilizationId { get; set; }

        [Required]
        public int HealthCareCenterId { get; set; }
        public IFormFile ProfileImage { get; set; }

        public IEnumerable<SelectListItem> Specalizations { get; set; }=new List<SelectListItem>();
        public IEnumerable<SelectListItem> HealthCareCenters { get; set; } = new List<SelectListItem>();
    }
}
