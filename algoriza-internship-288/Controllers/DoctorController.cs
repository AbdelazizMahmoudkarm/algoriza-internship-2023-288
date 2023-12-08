using algoriza_internship_288.Domain.Models.Enums;
using Domain.DtoClasses.Appointment;
using Domain.DtoClasses.Doctor;
using Service.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.UnitOfWork;

namespace algoriza_internship_288.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   [Authorize(Roles =nameof(UserType.Doctor) +","+ nameof(UserType.Admin))]
    public class DoctorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DoctorController(IUnitOfWork unitOfWork)
        =>  _unitOfWork = unitOfWork;
        
        [HttpGet("GetAll")]
        [Authorize(Roles = nameof(UserType.Admin)+","+nameof(UserType.Patient))]
        public IActionResult GetAll(int pageIndex = 1,int pageSize=5,string searchIdOrName=null)
        {
            IQueryable<GetDoctorDto> doctors;
            if (!searchIdOrName.IsNullOrEmpty())
            {
                int.TryParse(searchIdOrName, out int doctorId);
                if (doctorId > 0)
                    doctors = _unitOfWork.Doctor.GetByCondition(x => x.Id == doctorId);
                else
                    doctors = _unitOfWork.Doctor.GetByCondition(x =>
                    x.User.UserName.Contains(searchIdOrName) || x.User.Email.Contains(searchIdOrName));
            }
            else
                doctors = _unitOfWork.Doctor.GetByCondition(default);
            return Ok(doctors.Paginate(pageIndex,pageSize));
        }
        

        [HttpGet("GetById/{id:int}")]
        [Authorize(Roles = nameof(UserType.Admin))]
        public IActionResult GetById(int id)
        {
            if (id == 0)
                return BadRequest();
            GetDoctorDto doctor = _unitOfWork.Doctor.GetByCondition(x => x.Id == id).FirstOrDefault();
            if (doctor == null)
                return Ok("No data");
            else
                return Ok(doctor);
        }
        [HttpPost("Add")]
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> Add([FromForm]AddDoctorDto doctorDto)
        {
            
            if (!ModelState.IsValid)
                return BadRequest();
            bool result = await _unitOfWork.Doctor.AddAsync(doctorDto);
            return Ok(result);   
        }
        [HttpPut("Update")]
        [Authorize(Roles = nameof(UserType.Doctor))]
        public  async Task<IActionResult> Update([FromForm]UpdateDoctorDto editDoctorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
           bool result = _unitOfWork.Doctor.UpdateDoctor(editDoctorDto);
            if (result)
                await _unitOfWork.SaveAsync();
            return Ok(result);
        }

        [HttpPost("AddDoctorAppointments")]
        [Authorize(Roles = nameof(UserType.Doctor))]
        public async Task<IActionResult> AddAppointments([FromBody] AddDoctorAppointmentsDto appointmentModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (!await _unitOfWork.Doctor.UpdateWithAppointmentAddAsync(appointmentModel, User.Identity.Name))
                return NotFound();
            return Ok(true);
        }
        [HttpPut("UpdateTime")]
        [Authorize(Roles = nameof(UserType.Doctor))]
        public async Task<IActionResult> UpdateAppointMentAsync(UpdateAppointmentDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            bool reault = await _unitOfWork.Doctor.UpdateAppointmentTimeAsync(model, User.Identity.Name);
            if (reault)
                await _unitOfWork.SaveAsync();
            return Ok(reault);
        }
        [HttpDelete("DeleteTime")]
        [Authorize(Roles = nameof(UserType.Doctor))]
        public async Task<IActionResult> DeleteTimeAsync(int timeId)
        {
            if(timeId==0)
                return BadRequest();
            bool result = await _unitOfWork.Doctor.DeleteAppointmentAsync(timeId, User.Identity.Name);
            if (result)
                await _unitOfWork.SaveAsync();
            return Ok(result);
        }
    }
}
