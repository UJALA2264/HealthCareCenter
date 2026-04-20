using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookAppointmentMVC.Models
{
    public class PatientAppointment
    {
        public int AppointmentId { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan SlotStartTime { get; set; }
        public string Status { get; set; }

    }
}
