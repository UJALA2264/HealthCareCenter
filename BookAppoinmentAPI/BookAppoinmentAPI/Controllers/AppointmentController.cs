using BookAppoinmentAPI.DTOs;
using BookAppoinmentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookAppoinmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : Controller
    {
        private readonly HealthCareContext _http;

        public  AppointmentController(HealthCareContext http)
        {
            this._http = http;
        }

        [HttpPost("BookSlot")]
        public async Task<IActionResult> BookSlot([FromBody] BookSlotDto books)
        {
            using var transaction = await _http.Database.BeginTransactionAsync();
            var exist = await _http.Appointments
                .AnyAsync(a=> a.DoctorId ==books.DoctorId
                && a.Availability.Date ==books.Date.Date
                && a.Availability.SlotStartTime == books.StartTime
                //&& a.AppointmentStatus.Name=="Booked"
                && new[] {1,2}.Contains(a.AppointmentStatusId)
                );
            if (exist) return BadRequest("Slot Already Booked");
            try
            {
                var availability = new Availability
                {
                    DoctorId = books.DoctorId,
                    Date = books.Date,
                    SlotStartTime = books.StartTime,
                    SlotEndTime = books.StartTime.Add(TimeSpan.FromMinutes(30)),
                };
                _http.Availabilities.Add(availability);
                await _http.SaveChangesAsync();

                var appointment = new Appointment
                {
                    DoctorId = books.DoctorId,
                    EmployeeId = books.EmployeeId,
                    AvailabilityId = availability.AvailabilityId,
                    AppointmentStatusId = 1, // Booked
                    CreatedAt = DateTime.Now.ToString()
                };
                _http.Appointments.Add(appointment);
                await _http.SaveChangesAsync();
                await transaction.CommitAsync(); // <-- important
                return Ok(new { message = "Appointment booked", appointmentId = appointment.AppointmentId });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Internal error: " + ex.Message);
            }
        }

        [HttpGet("GetAppointments")]
        public async Task<IActionResult> GetAppointments()
        {
            var appointment = await _http.Appointments.Include(a => a.Doctor)
                .Include(a => a.Employee)
                .Include(a => a.Availability)
                .Include(a => a.AppointmentStatus)
                .Where(a => a.Availability.Date > DateTime.Now)
                .Select(a => new
                {
                    a.AppointmentId
                    ,
                    DoctorName = a.Doctor.FullName
                    ,
                    PatientName = a.Employee.Name
                    ,
                    a.Availability.Date
                    ,
                    a.Availability.SlotStartTime
                    ,
                    StatusId = a.AppointmentStatusId
                }).ToListAsync();

            return Ok(appointment);
        }

        [HttpGet("GetAppointmentsByPatient/{EmpId}")]
        public async Task<IActionResult> GetAppointmentsByPatient(int EmpId)
        {
            if (EmpId == 0) return BadRequest("Invalid Person");
            var appointment = await _http.Appointments.Include(a => a.Doctor)
                .Include(a => a.Employee)
                .Include(a => a.Availability)
                .Include(a => a.AppointmentStatus)
                .Where(a => a.Availability.Date > DateTime.Now && a.EmployeeId==EmpId)
                .Select(a => new
                {
                    a.AppointmentId
                    ,
                    DoctorName = a.Doctor.FullName
                    ,
                    PatientName = a.Employee.Name
                    ,
                    a.Availability.Date
                    ,
                    a.Availability.SlotStartTime
                    ,
                    Status = a.AppointmentStatus.Name
                }).ToListAsync();

            return Ok(appointment);
        }

        [HttpGet("GetStatus")]

        public async Task<IActionResult> GetStatus()
        {
            var lst = await _http.AppointmentStatus.OrderBy(a => a.Name).Select(a => new
            {
                id = a.StatusId,
                Name = a.Name
            }).ToListAsync();
            return Ok(lst);
        }


        [HttpPost("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusDto obj)
        {
            if (obj.AppointmentId == 0 || obj.StatusId == 0) return BadRequest("Select Value");
            var res = await _http.Appointments
                .Where(a => a.AppointmentId == obj.AppointmentId)
                .ExecuteUpdateAsync(s => s.SetProperty(a => a.AppointmentStatusId, obj.StatusId));
           

            if (res > 0)
            {
                var appointment = await _http.Appointments
               .Where(a => a.AppointmentId == obj.AppointmentId)
               .Select(a => new {
                   a.AppointmentId,
                   StatusId = a.AppointmentStatusId,
                   DoctorName = a.Doctor.FullName,
                   PatientName = a.Employee.Name,
                   PatientEmail = a.Employee.EmailId,
                   Date = a.Availability.Date,
                   Time = a.Availability.SlotStartTime

               }).FirstOrDefaultAsync();
                var payload = new
                {
                    AppointmentId= appointment.AppointmentId,
                    StatusId= appointment.StatusId,
                    DoctorName= appointment.DoctorName,
                    PatientName= appointment.PatientName,
                    PatientEmail= appointment.PatientEmail,
                      Date= appointment.Date.ToString("yyyy-MM-dd"),
                    Time= appointment.Time.ToString(@"hh\:mm")
                };
                using var azurehttp = new HttpClient();
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await azurehttp.PostAsync("https://prod-33.eastus.logic.azure.com:443/workflows/7e4f037f36af48f7a5ac9c0c1b0e88e2/triggers/When_an_HTTP_request_is_received/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2FWhen_an_HTTP_request_is_received%2Frun&sv=1.0&sig=ZCvHJuU62nFylDz-EoLg70VeMDiy4kUMwpQQ3WyHCHo", content);
                return Ok("Status updated successfully!");

            }
            else
            {
                return BadRequest("Failed !");
            }
        }
    }
}
