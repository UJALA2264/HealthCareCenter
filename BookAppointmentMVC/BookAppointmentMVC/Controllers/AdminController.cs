using BookAppointmentMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;
using System.Net.WebSockets;


namespace BookAppointmentMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _http;

        public AdminController(IHttpClientFactory http)
        {
            _http = http.CreateClient("BaseUrl");
            
        }
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult>AddDoctor()
        {
            var obj = new DoctorViewModel();
            var specResponse = await _http.GetFromJsonAsync<List<ApiItem>>("Api/Specialization/GetSpecilizations");
            obj.Specalizations = specResponse.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name });

            var list1 = await _http.GetFromJsonAsync<List<ApiItem>>("Api/HealthCenter/HealthCenters");
            obj.HealthCareCenters = list1.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Name,
            });
            return View(obj);
        }


        [HttpPost]
        public async Task<IActionResult> AddDoctor(DoctorViewModel obj)
        {
            if (!ModelState.IsValid) return View(obj);

            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(obj.FullName), "FullName");
            content.Add(new StringContent(obj.Qualifications), "Qualifications");
            content.Add(new StringContent(obj.ExperienceYears.ToString()), "ExperienceYears");
            content.Add(new StringContent(obj.Summary), "Summary");
            content.Add(new StringContent(obj.SpecilizationId.ToString()), "SpecilizationId");
            content.Add(new StringContent(obj.HealthCareCenterId.ToString()), "HealthCareCenterId");

            if(obj.ProfileImage !=null && obj.ProfileImage.Length > 0)
            {
                var filecontent = new StreamContent(obj.ProfileImage.OpenReadStream());
                filecontent.Headers.ContentType=new System.Net.Http.Headers.MediaTypeHeaderValue(obj.ProfileImage.ContentType);
                content.Add(filecontent, "ProfileImage", obj.ProfileImage.FileName);
            }

                var res = await _http.PostAsync("Api/Doctor/AddDoctor", content);
                if (res.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Doctor Added Successfully !";
                    return RedirectToAction("DoctorsList", "Common");
                }
            
            ModelState.Clear();
            ModelState.AddModelError("", "Failed to Add Doctor");
            var specResponse = await _http.GetFromJsonAsync<List<ApiItem>>("Api/Specialization/GetSpecilizations");
                obj.Specalizations = specResponse.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name });

                var list1 = await _http.GetFromJsonAsync<List<ApiItem>>("Api/HealthCenter/HealthCenters");
                obj.HealthCareCenters = list1.Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Name,
                });
                return View(obj);
        }
        
        [HttpGet]
        public async Task<IActionResult> AddHealthCenter()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddHealthCenter(HealthCenterViewModel obj)
        {
            if (!ModelState.IsValid) return View(obj);
            var res=await _http.PostAsJsonAsync("Api/HealthCenter/AddHealthCenter", obj);
            if (res.IsSuccessStatusCode)
            {
                TempData["Success"] = "Health Center Added Successfully !";
                return View();
            }

            ModelState.AddModelError("", "Failed to Add Health Center");
            return View(obj);
        }
        
       public async Task<IActionResult> GetAppointments()
        {

            var lst = await _http.GetFromJsonAsync<List<AppointmentDetails>>("Api/Appointment/GetAppointments");
            var statuslist = await _http.GetFromJsonAsync<List<ApiItem>>("Api/Appointment/GetStatus");
            if (lst != null)
            {
                foreach (var item in lst)
                {
                    item.statusOptions = statuslist.Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Name,
                        Selected = (e.Id == item.StatusId)
                    });
                }
            }

            return View(lst);
        }

        public async Task<IActionResult> UpdateStatus(int AppointmentId,int StatusId)
        {
            var obj = new
            {
                AppointmentId = AppointmentId,
                StatusId = StatusId
            };
            var res = await _http.PostAsJsonAsync("Api/Appointment/UpdateStatus", obj);
            return RedirectToAction("GetAppointments");
        }
    }
}
