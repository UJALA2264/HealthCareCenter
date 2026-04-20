using BookAppoinmentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookAppoinmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecializationController : Controller
    {
        private readonly HealthCareContext context;

       public SpecializationController(HealthCareContext context)
        {
            this.context = context;
        }
        [HttpPost("AddSpecilization")]
        public async Task<IActionResult> AddSpecilization([FromBody] Specilization obj)
        {
            if (obj == null) return BadRequest();
            await context.Specilizations.AddAsync(obj);
            await context.SaveChangesAsync();
            return Ok("Specilization Added Successfully!");
        }

        [HttpGet("GetSpecilizations")]
        public async Task<IActionResult> GetListofSpecilizations()
        {
            var list = await context.Specilizations
                .OrderBy(e => e.Name)
                .Select(s => new { id = s.SpecilizationId, name = s.Name }).ToListAsync();
            return Ok(list);
        }

        
    }
}
