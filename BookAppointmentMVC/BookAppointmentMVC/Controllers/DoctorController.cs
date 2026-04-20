using Microsoft.AspNetCore.Mvc;

namespace BookAppointmentMVC.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
