using Azure.Storage.Blobs;
using BookAppoinmentAPI.DTOs;
using BookAppoinmentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BookAppoinmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : Controller
    {
        private readonly HealthCareContext healthCareContext;
        private readonly IConfiguration _config;
        public DoctorController(HealthCareContext healthCareContext, IConfiguration config)
        {
            this.healthCareContext = healthCareContext;
            this._config = config;
        }

        [HttpGet("AllDoctors")]
        public async Task<IActionResult> ListOfDoctors()
        {
            var exist = await healthCareContext.Doctors.Include(d => d.Specilization)
                .Include(d => d.HealthCareCenter).Select(d => new
                {
                    d.DoctorId,
                    d.FullName,
                    d.Qualifications,
                    d.ExperienceYears,
                    CenterName = d.HealthCareCenter.Name,
                    SpecalizationName = d.Specilization.Name,
                    d.ProfileName,
                    d.Summary
                }).ToListAsync();
            if (exist.Count == 0) return BadRequest("There is Not any Doctor!");
            return Ok(exist);
        }

        //Search Doctor By center, specialization, name
        [HttpGet("DoctorById/{doctorId}")]
        public async Task<IActionResult> DoctorsById(int doctorId)
        {
            var doc = await healthCareContext.Doctors
        .Include(d => d.Specilization)
        .Include(d => d.HealthCareCenter)
        .FirstOrDefaultAsync(d => d.DoctorId == doctorId);

            if (doc == null) return NotFound();

            return Ok(new
            {
                doc.DoctorId,
                doc.FullName,
                doc.ExperienceYears,
                doc.Qualifications,
                doc.Summary,
                ImageUrl = doc.ProfileName,
                Specialization = doc.Specilization?.Name,
                Center = doc.HealthCareCenter?.Name
            });
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchDoctors([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest();

            query = query.Trim().ToLower();
            var doctors = await healthCareContext.Doctors.AsNoTracking()
                .Include(d => d.Specilization)
                .Include(d => d.HealthCareCenter)
                .Where(d => d.HealthCareCenter.IsActive && d.Specilization.IsActive
                && (d.FullName.ToLower().Contains(query) ||
                d.Specilization.Name.ToLower().Contains(query) ||
                d.HealthCareCenter.Name.ToLower().Contains(query)
                )).Select(d => new
                {
                    d.DoctorId,
                    d.FullName,
                    SpecilizationName = d.Specilization.Name,
                    d.ExperienceYears,
                    d.Qualifications,
                    CenterName = d.HealthCareCenter.Name,
                    d.ProfileName,
                    d.Summary
                }).ToListAsync();
            return Ok(doctors);
        }

        [HttpPost("AddDoctor")]
        public async Task<IActionResult> AddDoctor([FromForm] DoctorDto doc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (doc == null) return BadRequest("Wrong Input");
            string imageUrl = null;
            try
            {
                // Upload image to Azure Blob Storage (if provided)
                if (doc.ProfileImage != null && doc.ProfileImage.Length > 0)
                {
                    var connStr = _config["AzureStorage:ConnectionString"];
                    var containerName = _config["AzureStorage:ContainerName"];

                    var container = new BlobContainerClient(connStr, containerName);
                    await container.CreateIfNotExistsAsync();

                    // Optional: set public access if you want direct URL visibility (skip if container is private)
                    // await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

                    var safeExt = Path.GetExtension(doc.ProfileImage.FileName);
                    var fileName = $"{Guid.NewGuid()}{safeExt}";
                    var blob = container.GetBlobClient(fileName);

                    // Preserve content type
                    var headers = new Azure.Storage.Blobs.Models.BlobHttpHeaders
                    {
                        ContentType = doc.ProfileImage.ContentType
                    };

                    using var stream = doc.ProfileImage.OpenReadStream();
                    await blob.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobUploadOptions
                    {
                        HttpHeaders = headers
                    });

                    imageUrl = blob.Uri.ToString(); // full URL to blob
                }

                var obj = new Doctor
                {
                    FullName = doc.FullName,
                    ExperienceYears = doc.ExperienceYears,
                    Qualifications = doc.Qualifications,
                    Summary = doc.Summary,
                    SpecilizationId = doc.SpecilizationId,
                    HealthCareCenterId = doc.HealthCareCenterId,
                    ProfileName = imageUrl
                };
                await healthCareContext.AddAsync(obj);
                await healthCareContext.SaveChangesAsync();
                return Ok("Doctor Added Successfully!");

            }
            catch (Exception ex)
            {
                // Log the error in real apps; here return details for debugging
                return Problem(statusCode: 500, title: "Upload failed", detail: ex.Message);
            }
        }

}
}
