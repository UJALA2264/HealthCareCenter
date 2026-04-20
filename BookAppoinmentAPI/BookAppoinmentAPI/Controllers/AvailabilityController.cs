using BookAppoinmentAPI.DTOs;
using BookAppoinmentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookAppoinmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvailabilityController : Controller
    {
        private readonly HealthCareContext context;

        public AvailabilityController(HealthCareContext context)
        {
            this.context = context;
        }

        [HttpPost("AddAvailability")]
        public async Task<IActionResult> AddAvailability([FromBody] AvailabilityDto availability)
        {
            if (availability == null) return BadRequest("Enter valid Input!");
            var obj = new Availability
            {
                DoctorId = availability.DoctorId,
                Date = availability.Date,
                SlotStartTime = availability.SlotStartTime,
                SlotEndTime = availability.SlotEndTime
            };
            await context.Availabilities.AddAsync(obj);
            await context.SaveChangesAsync();
            return Ok("Availability Added Successfully !");
        }

        [HttpGet("GetAvailability/{doctorId}")]
        public async Task<IActionResult> GetAvailability(int doctorId)
        {
            var Today = DateTime.Today;
            var endDate = Today.AddDays(7);

            var slots = new List<AvailabilityDto>();
            var slotDuration = TimeSpan.FromMinutes(30);
            for (var date = Today.AddDays(1); date <= endDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Sunday) continue;
                var start = new TimeSpan(9, 0, 0);
                var end = new TimeSpan(17, 0, 0);

                for (var time = start; time < end; time += slotDuration)
                {
                    var booked = await context.Appointments.AnyAsync(a => a.DoctorId == doctorId &&
                    a.Availability.Date == date
                    && a.Availability.SlotStartTime == time
                    && a.AppointmentStatus.Name == "Pending" || a.AppointmentStatus.Name == "");
                    if (!booked)
                    {
                        slots.Add(new AvailabilityDto
                        {
                            DoctorId = doctorId,
                            Date = date,
                            SlotStartTime = time,
                            SlotEndTime = time + slotDuration,
                            Status = "Available"
                        });
                    }
                }
            }
       
         return Ok(slots);
        }


        
    }
}
