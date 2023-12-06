using algoriza_internship_288.Domain.Models.Enums;
using Domain.DtoClasses.Booking;
using Domain.DtoClasses.Doctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;

namespace algoriza_internship_288.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(UserType.Admin))]
    public class DashBoardController : ControllerBase
    {
        public readonly IUnitOfWork _unitOfWork;
        public DashBoardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("NumOfDoctor")]
        public async Task<IActionResult> DoctorNum(DateTime date)
        {
            int numOfDoctors = await _unitOfWork.Doctor.CountAsync(UserType.Doctor.ToString(),date);
            return Ok(numOfDoctors);
        }

        [HttpGet("NumOfPatient")]
        public async Task<IActionResult> PatientNum(DateTime date)
        {
            int numberOfPatient = await _unitOfWork.Doctor.CountAsync(UserType.Patient.ToString(),date);
            return Ok(numberOfPatient);
        }

        [HttpGet("NumberOfRequestes")]
        public IActionResult RequestNumber(DateTime date)
        {
            List<dynamic> RequestsWithStatus = _unitOfWork.Booking.GetStatusWithRequestNumber(date).ToList();
            return Ok(RequestsWithStatus);
        }

        [HttpGet("Top5Specialization")]
        public IActionResult Top5Specialization(DateTime date)
        {
            List<GetBookingInfoDto> top5OfDocIdWIthNumOfRequests = _unitOfWork.Booking
                .GetDoctorIdWithNumberOfRequests(5, date).ToList();

            Dictionary<string, int> SpeclizeWithRequestNumber = new();
            foreach (GetBookingInfoDto request in top5OfDocIdWIthNumOfRequests)
                SpeclizeWithRequestNumber
                    .Add(
                    _unitOfWork.Specialization.GetSpecializeNameByDoctorId(request.DoctorId)
                    , request.NumberOfRequests);

            return Ok(SpeclizeWithRequestNumber);
        }
        [HttpGet("Top10Doctors")]
        public IActionResult Top10DoctorsWithSpecialization(DateTime date)
        {
            List<GetBookingInfoDto> Top5OfDocIdWIthNumOfRequests = _unitOfWork.Booking
                .GetDoctorIdWithNumberOfRequests(10, date).ToList();

            List<DoctorFilterIntoDto> DoctorWithRequestNumber = new();
            foreach (GetBookingInfoDto request in Top5OfDocIdWIthNumOfRequests)
            {
                DoctorFilterIntoDto doctorWithRequestNumber = _unitOfWork.Doctor
                    .GetDoctorInfoWithSpecializeName(request.DoctorId);
                doctorWithRequestNumber.RequestsNumber=request.NumberOfRequests;
                DoctorWithRequestNumber.Add(doctorWithRequestNumber);
            }
            return Ok(DoctorWithRequestNumber);
        }
        
    }
}
