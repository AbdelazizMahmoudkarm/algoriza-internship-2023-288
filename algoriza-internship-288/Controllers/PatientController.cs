using algoriza_internship_288.Domain.Models.Enums;
using Domain.DtoClasses.Patient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;

namespace algoriza_internship_288.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(UserType.Patient) + "," + nameof(UserType.Admin))]
    public class PatientController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public PatientController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] AddPatientDto patientModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            bool result = await _unitOfWork.Patient.AddAsync(patientModel);
            return Ok(result);
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

