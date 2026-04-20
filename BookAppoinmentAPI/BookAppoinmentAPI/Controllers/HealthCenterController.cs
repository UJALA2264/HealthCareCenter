using BookAppoinmentAPI.DTOs;
using BookAppoinmentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookAppoinmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCenterController :ControllerBase
    {
       private readonly HealthCareContext healthCareContext;

        public HealthCenterController(HealthCareContext healthCareContext)
        {
            this.healthCareContext = healthCareContext;
        }
        [HttpGet("HealthCenters")]
        public async Task<IActionResult> ListOfCenters()
        {
            var list = await healthCareContext.healthCareCenters.Where(e=>e.IsActive==true)
                .OrderBy(e => e.Name).Select(e => new
                {
                    id = e.CenterId,
                    Name = e.Name,
                }).
                ToListAsync();
            return Ok(list);
        }

        [HttpPost("AddHealthCenter")]
        public  async Task<IActionResult> AddHealthCenter([FromBody] HealthCenterDto center)
        {
            if (center == null) return BadRequest("Enter Details !");
            var obj = new HealthCareCenter
            {
                Name = center.Name,
                Addess = center.Addess,
                City = center.City,
                Pincode = center.Pincode
            };
           await healthCareContext.healthCareCenters.AddAsync(obj);
            await healthCareContext.SaveChangesAsync();
            return Ok("HealthCenter Added Successfully !");
        }

        [HttpGet("CenterById")]
        public async Task<IActionResult> GetCenterById(int id) {

            if (id == 0) return BadRequest("Enter Id");
            return Ok(await healthCareContext.healthCareCenters
                .SingleOrDefaultAsync(a => a.CenterId == id));
        }

    }
}
