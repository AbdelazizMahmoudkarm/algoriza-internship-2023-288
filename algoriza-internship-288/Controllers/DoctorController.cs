using Domain.DtoClasses.Appointment;
using Domain.DtoClasses.Doctor;
using EF.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.UnitOfWork;
using System;

namespace algoriza_internship_288.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public DoctorController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = webHost;
        }
        [HttpGet("GetAll")]///for admin
        public IActionResult GetAll(int pageIndex,int pageSize,string? search=null)
        {
            IQueryable<GetDoctorDto> doctors;
            if (!search.IsNullOrEmpty())
            {
                int.TryParse(search, out int doctorId);
                if (doctorId > 0)

                    doctors = _unitOfWork.Doctor.GetByCondition(x => x.Id == doctorId);
                else
                    doctors = _unitOfWork.Doctor.GetByCondition(x =>
                    x.User.UserName.Contains(search) || x.User.Email.Contains(search));
            }
            else
                doctors = _unitOfWork.Doctor.GetByCondition(default);
            return Ok(doctors.Paginate(pageIndex,pageSize));
        }
        

        [HttpGet("GetById/{id:int}")]
        public IActionResult GetById(int id)
        {
            if (id == 0)
                return BadRequest();
           GetDoctorDto doctor= _unitOfWork.Doctor.GetByCondition(x => x.Id == id).FirstOrDefault();
            if (doctor == null)
                return Ok("No data");
            else
                return Ok(doctor);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm]AddDoctorDto doctorDto)
        {
            bool isOk = false;
            if(ModelState.IsValid)
                isOk = await _unitOfWork.Doctor.AddAsync(doctorDto);
            return Ok(isOk);   
        }
        [HttpPut("Edit")]
        public  async Task<IActionResult> Edit([FromForm]UpdateDoctorDto editDoctorDto)
        {
            bool isOk = false;
            if (ModelState.IsValid)
            {
                isOk = _unitOfWork.Doctor.UpdateDoctor(editDoctorDto);
                if (isOk)
                    try
                    {
                        await _unitOfWork.SaveAsync();
                    }catch(Exception ex)
                    {

                    }
            }
            return Ok(isOk);
        }

        [HttpPost("AddDoctorAppointments")]
        public async Task<IActionResult> AddAppointments([FromBody] AddDoctorAppointmentsDto appointmentModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            string userName = User.Identity.Name;
            if (!await _unitOfWork.Doctor.UpdateWithAppointmentAddAsync(appointmentModel, "AliMahmoud"))
                return NotFound();
            return Ok(true);
        }
        [HttpPut("UpdateTime")]
        public async Task<IActionResult> UpdateAppointMentAsync(EditAppointmentDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = User.Identity.Name;
            bool reault = await _unitOfWork.Doctor.UpdateAppointmentTimeAsync(model, user);
            if (reault) await _unitOfWork.SaveAsync();
            return Ok(reault);
        }
        [HttpDelete("DeleteTime")]
        public async Task<IActionResult> DeleteTimeAsync(int timeId)
        {
            string userName = User.Identity.Name;
            bool result = await _unitOfWork.Doctor.DeleteAppointmentAsync(timeId, userName);
            if (result)
                await _unitOfWork.SaveAsync();
            return Ok(result);
        }


        //[HttpDelete("Delete")]
        //public async Task<IActionResult> DeleteAsync(int id)
        //{
        //    bool isOk = await _unitOfWork.Doctor.Delete(id);
        //    if (isOk)
        //        await _unitOfWork.SaveAsync();
        //    return Ok(isOk);
        //}
    }
}
