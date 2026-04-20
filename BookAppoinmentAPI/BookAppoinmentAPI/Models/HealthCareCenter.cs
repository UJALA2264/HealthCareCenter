using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookAppoinmentAPI.Models
{
    public class HealthCareCenter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CenterId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Addess { get; set; }

        [Required]
        public string City { get; set; }

        [JsonIgnore]
        public ICollection<Doctor>Doctors { get; set; }

        [Required]
        public string Pincode { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
