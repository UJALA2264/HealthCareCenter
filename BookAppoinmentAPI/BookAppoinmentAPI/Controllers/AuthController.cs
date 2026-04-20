using BookAppoinmentAPI.DTOs;
using BookAppoinmentAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookAppoinmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly HealthCareContext _healthCareContext;

        public AuthController(HealthCareContext healthCareContext)
        {
            _healthCareContext = healthCareContext;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto reg)
        {
            var exist = await _healthCareContext.LoginUsers.AnyAsync(u => u.EmailId == reg.Email);
            if (exist)
            {
                return BadRequest("User already exists");
            }
            var user1 = _healthCareContext.Employees.SingleOrDefault(e => e.EmailId == reg.Email);
            if (user1 == null) return BadRequest("Only HCL Employee Can Register !");

           
            var user = new LoginUser
            {
                Username = reg.Name,
                EmailId = reg.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(reg.Password),
                EmployeeId = user1.EmployeeId,
                Role = reg.Role
            };

            await _healthCareContext.LoginUsers.AddAsync(user);
           await _healthCareContext.SaveChangesAsync();
            return Ok("User Register Successfully !");
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            var user = _healthCareContext.LoginUsers.Include(a=>a.Employee).SingleOrDefault(e => e.EmailId == login.Email);
            if (user == null) return Unauthorized("User Not Register !");

            if (!BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash)) return Unauthorized("Invalid Password !");

            return Ok(new LoginUserDto
            {
                UserId=user.UserId,
                FullName=user.Employee.Name,
                Role=user.Role
            });
        }
    }
}
