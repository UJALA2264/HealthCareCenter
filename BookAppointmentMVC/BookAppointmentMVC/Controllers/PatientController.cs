using BookAppointmentMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Numerics;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookAppointmentMVC.Controllers
{

    public class PatientController : Controller
    {
        private readonly HttpClient _http;
        public PatientController(IHttpClientFactory http)
        {
            _http = http.CreateClient("BaseUrl");

        }
        public async Task<IActionResult> Dashboard()
        {
            int EmpId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (EmpId == 0)
            {
                // Session expired or not set
                return RedirectToAction("Login", "Account");
            }

            var lst = await _http.GetFromJsonAsync<List<PatientAppointment>>($"Api/Appointment/GetAppointmentsByPatient/{EmpId}");

            return View(lst);
        }
        public async Task<IActionResult> BookAppointment(int doctorId)
        {
            var doctors = await _http.GetFromJsonAsync<DoctorDetailsVm>($"Api/Doctor/DoctorById/{doctorId}");
            if (doctors == null) return NotFound();

            var slots = await _http.GetFromJsonAsync<List<AvailabilitySlotVm>>($"Api/Availability/GetAvailability/{doctorId}");
            doctors.Slots = slots ?? new List<AvailabilitySlotVm>();

            return View(doctors);
        }
    }
}
