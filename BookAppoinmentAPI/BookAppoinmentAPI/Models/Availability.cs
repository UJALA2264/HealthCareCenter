using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAppoinmentAPI.Models
{
    public class Availability
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AvailabilityId {  get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date {  get; set; }
        [Required]
        [DataType(DataType.Time)]
        public TimeSpan SlotStartTime {  get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan SlotEndTime {  get; set; }
        public Doctor Doctor { get; set; }
        public int DoctorId {  get; set; }

        public Appointment appointment { get; set; }
    }
}
