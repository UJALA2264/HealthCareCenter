using BookAppointmentMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookAppointmentMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _http;
        public AccountController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("BaseUrl");
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var payload = new { Email = email, Password = password };
            var res = await _http.PostAsJsonAsync("api/auth/login", payload);

            if (!res.IsSuccessStatusCode)
            {
                TempData["Error"] = "Invalid credentials";
                return RedirectToAction("LoginForm");
            }

            var user = await res.Content.ReadFromJsonAsync<LoginUserDto>();

            // Store in session
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetString("UserName", user.FullName);

            // Redirect based on role
            if (user.Role == "Admin") return RedirectToAction("Dashboard", "Admin");
            if (user.Role == "Doctor") return RedirectToAction("Dashboard", "Doctor");
            return RedirectToAction("Dashboard", "Patient");
        }


        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password, string role)
        {
            var payload = new RegisterDto
            {
                Name = name,
                Email = email,
                Password = password,
                Role = role
            };
            var res = await _http.PostAsJsonAsync("api/Auth/Register", payload);

            if (!res.IsSuccessStatusCode)
            {
                TempData["Error"] = "Registration failed";
                return RedirectToAction("Login");
            }

            TempData["Success"] = "Registration successful, please login.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
