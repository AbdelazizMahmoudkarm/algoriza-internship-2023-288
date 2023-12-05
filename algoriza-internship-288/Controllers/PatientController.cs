using Domain.DtoClasses.Patient;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;

namespace algoriza_internship_288.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public PatientController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] AddPatientDto patientModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (patientModel is null)
                return NotFound();
            bool result = await _unitOfWork.Patient.AddAsync(patientModel);
            return Ok(result);
        }
        private string ProcessImage(IFormFile photo)
        {
            string uniqueName = default;
            if (photo is not null)
            {
                string path = Path.Combine(_hostEnvironment.WebRootPath, "Images");
                uniqueName = Guid.NewGuid() + "_" + photo.FileName;
                string Fullpath = Path.Combine(path, uniqueName);
                using FileStream fileStream = new(Fullpath, FileMode.Create);
                photo.CopyTo(fileStream);
            }
            return uniqueName;
        }
        //private int GetAge(DateTime dob)
        //{
        //    int age = DateTime.Now.Year - dob.Year;
        //    if (DateTime.Now.DayOfYear < dob.DayOfYear)
        //        age -= 1;
        //    return age;
        //}
    }
}

