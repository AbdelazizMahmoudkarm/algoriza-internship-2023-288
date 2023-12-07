using algoriza_internship_288.Domain.Models;
using algoriza_internship_288.Domain.Models.Enums;
using Domain.DtoClasses.Booking;
using Domain.DtoClasses.Patient;
using Service.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;

namespace algoriza_internship_288.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        [HttpGet("GetAllRequestsForDoctor")]
        [Authorize(Roles = nameof(UserType.Doctor))]
        public async Task<IActionResult> GetAllRequestsAsync(DateTime date,int page = 1 , int pageSize=5)
        {
            string user = User.Identity.Name;
            IEnumerable<GetBookingForDoctorDto> DoctorBooking =
                   (await _unitOfWork.Booking.GetAllForDoctor(user, date))?.Paginate(page, pageSize);
            return Ok(DoctorBooking);

        }
        [HttpPut("ConfirmBooking")]
        [Authorize(Roles = nameof(UserType.Doctor))]
        public async Task<IActionResult> ConfirmAsync(int bookingId)
        {
            string user = User.Identity.Name;
            bool reault = await _unitOfWork.Booking.ConfirmAsync(bookingId, user);
            if (reault)
                await _unitOfWork.SaveAsync();
            return Ok(reault);
        }
        [HttpPost("Booking")]
        [Authorize(Roles = nameof(UserType.Patient))]
        public async Task<IActionResult> BookingAsync(AddBookingDto bookingModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            bool result = await _unitOfWork.Booking.AddAsync(bookingModel, User.Identity.Name);
            if (result)
                await _unitOfWork.SaveAsync();

            return Ok(result);
        }

        [HttpGet("GetAllBookingForPatient")]
        [Authorize(Roles = nameof(UserType.Patient))]
        public async Task<IActionResult> GetAllBookingForPatientAsync()
        {
            string userName = User.Identity.Name;
            return Ok(await _unitOfWork.Booking.GetAllForPatient(userName));
        }
        [HttpPut("CancelBooking")]
        [Authorize(Roles = nameof(UserType.Patient))]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            if (bookingId == 0)
                return BadRequest();
            string userNme = User.Identity.Name;
            bool result = await _unitOfWork.Booking.CancelAsync(bookingId, userNme);
            if (result)
                await _unitOfWork.SaveAsync();
            return Ok(result);
        }

    }
}
