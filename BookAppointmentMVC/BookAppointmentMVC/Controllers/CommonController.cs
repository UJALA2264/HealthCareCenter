using BookAppointmentMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookAppointmentMVC.Controllers
{
    public class CommonController : Controller
    {
        private readonly HttpClient _http;

        public CommonController(IHttpClientFactory http)
        {
            _http = http.CreateClient("BaseUrl");

        }
        [HttpGet]
        public async Task<IActionResult> DoctorsList()
        {
           var doctorslist=await  _http.GetFromJsonAsync<List<DoctorDetails>>("Api/Doctor/AllDoctors");
            if(doctorslist==null || !doctorslist.Any())
            {
                ViewBag.Message = "There is not any Doctor";
                return View(new List<DoctorDetails>());
            }
            return View(doctorslist);
        }
        [HttpPost]
        public async Task<IActionResult> DoctorsList(string? query)
        {
            try
            {
                query= query?.Trim();
                List<DoctorDetails>? doctors;
                if (string.IsNullOrWhiteSpace(query))
                {
                    doctors = await  _http.GetFromJsonAsync<List<DoctorDetails>>("Api/Doctor/AllDoctors");
                }
                else
                {
                    var encodedQuery = Uri.EscapeDataString(query);
                     doctors = await _http.GetFromJsonAsync<List<DoctorDetails>>($"Api/Doctor/Search?query={encodedQuery}");

                }

                doctors ??= new List<DoctorDetails>();

                if (!doctors.Any())
                {
                    ViewBag.Message = "There is not any Doctor";
                    return View(new List<DoctorDetails>());
                }

                return View(doctors);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Message = "Error fetching doctors: " + ex.Message;
                return View(new List<DoctorDetails>());
            }
        }


    [HttpPost]
        public async Task<IActionResult> ConfirmAppointment(int doctorId, DateTime date, TimeSpan startTime)
        {
            //var employeeId = 1;
            var employeeId = HttpContext.Session.GetInt32("UserId");
            if (employeeId == null)
            {
                TempData["Error"] = "You must be logged in to book an appointment.";
                return RedirectToAction("Login", "Account");
            }

            var payload = new
            {
                DoctorId = doctorId,
                EmployeeId = employeeId,
                Date = date.Date,
                StartTime = startTime
            };

            var res = await _http.PostAsJsonAsync("api/Appointment/BookSlot", payload);
            if (res.IsSuccessStatusCode)
            {
                TempData["Success"] = "Appointment booked!";
                return RedirectToAction("DoctorsList");
            }

            var msg = await res.Content.ReadAsStringAsync();
            TempData["Error"] = $"Booking failed: {msg}";
            return RedirectToAction("BookAppointment", new { doctorId });
        }
        public IActionResult viewHealthCenter()
        {
            return View();
        }
    }
}
