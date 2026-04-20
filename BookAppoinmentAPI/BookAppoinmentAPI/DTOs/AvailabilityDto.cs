using BookAppoinmentAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAppoinmentAPI.DTOs
{
    public class AvailabilityDto
    {

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public TimeSpan SlotStartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan SlotEndTime { get; set; }
        public int IsAvailable { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public string Status {  get; set; }
    }
}
