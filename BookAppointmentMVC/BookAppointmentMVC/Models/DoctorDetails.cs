namespace BookAppointmentMVC.Models
{
    public class DoctorDetails
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public int ExperienceYears { get; set; }
        public string Qualifications { get; set; }
        public string Summary { get; set; }
        public string ProfileName { get; set; } // Blob URL
        public string SpecilizationName { get; set; }
        public string CenterName { get; set; }
    }
}
