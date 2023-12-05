using algoriza_internship_288.Core.Models;
using Domain.DtoClasses.Booking;
using Domain.DtoClasses.Patient;
using EF.Pagination;
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

        [HttpGet("GetAllRequests")]
        public async Task<IActionResult> GetAllRequestsAsync(int page, int pageSize, DateTime date)
        {
            string user = User.Identity.Name;
            IEnumerable<GetBookingForDoctorDto> DoctorBooking =
                   (await _unitOfWork.Booking.GetAllForDoctor(user, date)).Paginate(page, pageSize);
            return Ok(DoctorBooking);

        }
        [HttpPut("ConfirmBooking")]
        public async Task<IActionResult> ConfirmAsync(int bookingId)
        {
            string user = User.Identity.Name;
            bool reault = await _unitOfWork.Booking.ConfirmAsync(bookingId, user);
            if (reault)
                await _unitOfWork.SaveAsync();
            return Ok(reault);
        }



        [HttpPost("Booking")]
        public async Task<IActionResult> BookingAsync(AddBookingDto bookingModel, string userName)
        {

            if (!ModelState.IsValid)
                return BadRequest();
            bool result = await _unitOfWork.Booking.AddAsync(bookingModel, userName);
            if (result)
                await _unitOfWork.SaveAsync();

            return Ok(result);
        }

        [HttpGet("GetAllBooking")]
        public async Task<IActionResult> GetAllBookingForPatientAsync()
        {
            string userName = User.Identity.Name;
            return Ok(await _unitOfWork.Booking.GetAllForPatient(userName));
        }
        [HttpPut("CancelBooking")]
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
