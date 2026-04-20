using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace BookAppointmentMVC.Models
{
    public class HealthCenterViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Addess { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Pincode { get; set; }

    }
}
