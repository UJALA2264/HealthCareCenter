using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAppoinmentAPI.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId {  get; set; }

         //(Booked/Cancelled/Rescheduled/SystemCancelled) 

        [Required]

        public string CreatedAt { get; set; } = DateTime.Now.ToString();


        public string UpdatedAt { get; set; } = "";

        //Foreign Keys 
        [Required]
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        [Required]
        public Doctor Doctor { get; set; }
        public int DoctorId { get; set; }

        public Availability Availability { get; set; }
        public int AvailabilityId {  get; set; }

        public AppointmentStatus AppointmentStatus { get; set; }
        public int AppointmentStatusId { get;  set; }

    }
}
