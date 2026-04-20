using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAppoinmentAPI.Models
{
    public class Specilization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpecilizationId { get; set; }

        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Doctor>Doctors { get; set; }

    }
}
