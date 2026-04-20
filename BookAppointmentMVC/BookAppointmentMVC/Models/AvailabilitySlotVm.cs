using System.ComponentModel.DataAnnotations;

namespace BookAppointmentMVC.Models
{
    public class AvailabilitySlotVm
    {
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
       
        public TimeSpan SlotStartTime { get; set; }

      
        public TimeSpan SlotEndTime { get; set; }
        public string Display => $"{Date:ddd, dd MMM} — {SlotStartTime:hh\\:mm}–{SlotEndTime:hh\\:mm}";
    }
}