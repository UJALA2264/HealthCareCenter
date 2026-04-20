using System.ComponentModel.DataAnnotations;

namespace BookAppoinmentAPI.DTOs
{
    public class HealthCenterDto
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
