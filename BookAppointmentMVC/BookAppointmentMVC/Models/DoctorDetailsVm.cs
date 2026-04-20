namespace BookAppointmentMVC.Models
{
    public class DoctorDetailsVm
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public int ExperienceYears { get; set; }
        public string Qualifications { get; set; }
        public string Summary { get; set; }
        public string ImageUrl { get; set; }
        public string Specialization { get; set; }
        public string Center { get; set; }
        public List<AvailabilitySlotVm> Slots { get; set; } = new();
    }
}
