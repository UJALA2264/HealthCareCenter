namespace BookAppoinmentAPI.DTOs
{
    public class BookSlotDto
    {
        public int DoctorId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
    }
}
