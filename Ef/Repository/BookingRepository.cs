using algoriza_internship_288.Domain.Models;
using algoriza_internship_288.Domain.Models.Enums;
using algoriza_internship_288.Repository.DAL;
using Domain.DtoClasses.Booking;
using Domain.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.IRepository;

namespace Repository.Repository
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        private readonly AppDbContext _context;
        private readonly ICouponRepository _coupon;
        private readonly IAppointmentRepository _appointment;
        private readonly ITimeRepository _time;
        private readonly UserManager<ApplicationUser> _userManager;
        public BookingRepository(AppDbContext context, UserManager<ApplicationUser> userManager,
            ICouponRepository coupon,
            IAppointmentRepository appointment, ITimeRepository time,SignInManager<ApplicationUser> signInManager) 
            : base(userManager, signInManager)
        {
            _context = context;
            _coupon = coupon;
            _appointment = appointment;
            _time = time;
            _userManager= userManager;
        }
        public async Task<bool> CancelAsync(int bookingId, string userName)
        {
            string patientId = (await GetUserAsync(UserType.Patient.ToString(), userName))?.Id;
            Booking book = _context.Bookings.FirstOrDefault(b => b.PatientId == patientId
            && b.Id == bookingId && b.Status.Equals(RequestType.Pending));
            if(book is not null)
            {
                book.Status = RequestType.Cancelled;
                return true;
            }
            return false;
        }

        public bool CheckIfDayAndTimeExists(int doctorId, int hourId)
        {
            if(doctorId != 0 && hourId !=0) 
            return _context.Bookings.Any(x => x.DoctorId == doctorId && x.HourId == hourId);
            return false;
        }
        public async Task<bool> ConfirmAsync(int bookingId, string userName)
        {
            int? Doctorid = (await GetUserAsync(UserType.Doctor.ToString(), userName))?.Doctor.Id;
            if (Doctorid.HasValue)
            {
                Booking book = await _context.Bookings.FirstOrDefaultAsync(b => b.DoctorId == Doctorid.Value
                                              && b.Id == bookingId && b.Status.Equals(RequestType.Pending));
                if (book is not null)
                {
                    book.Status = RequestType.Completed;
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> CheckIfCouponExistBookingAsync(int couponId)
            => await _context.Bookings.AnyAsync(x => x.CouponId == couponId);

        public async Task<bool> AddAsync(AddBookingDto book, string userName)
        {
            string patientId = (await GetUserAsync(UserType.Patient.ToString(), userName))?.Id;
            if (!patientId.IsNullOrEmpty())
            {
                Doctor doctor = await _context.Doctors.FindAsync(book.DoctorId);
                if (doctor is not null)
                {
                    int doctorId = doctor.Id;
                    //  dynamic appointment = _appointment.GetAppointmentIdWithTimeIdOrDefault(doctorId, book.Day, book.Time);
                    if (book.AppointmentId != 0 && book.TimeId != 0)
                    {
                        //int appointmentId = appointment.AppointmentId;
                        //int hourId = appointment.TimeId;

                        bool checkIfDayAndTimeNotAvalible = _context.Bookings
                            .Any(x => x.DoctorId == doctorId
                            && x.AppointmentId == book.AppointmentId && x.HourId == book.TimeId);
                        if (checkIfDayAndTimeNotAvalible)
                            return false;
                        double pricecheckOrRecheck = 0d;
                        if (!book.IsCheck)//Determine if check or recheck if the input is recheck
                        {
                            pricecheckOrRecheck = _context.Bookings
                                .Any(b => b.PatientId == patientId
                                && DateTime.Now.AddDays(-15) <= b.Date)// if the check more than 15 day you should check again not recheck
                                ? doctor.ReCheckPrice : doctor.CheckPrice;
                        }
                        else
                            pricecheckOrRecheck = doctor.CheckPrice;

                        int? couponId = null;
                        if (!book.CouponCode.IsNullOrEmpty())// if there is a coupon in input check how many booking for this patient
                        {
                            Coupon coupon = await _coupon.GetCouponByCode(book.CouponCode);
                            if (_context.Bookings
                                    .Count(booking => booking.PatientId == patientId
                                        && booking.CouponId.Equals(null)
                                        && booking.Status == RequestType.Completed)
                                                 >= coupon.NumberOfRequestCompleted)
                                couponId = coupon.Id;
                        }
                        _context.Bookings.Add(new Booking()
                        {
                            PatientId = patientId,
                            Date = DateTime.Now,
                            AppointmentId = book.AppointmentId,
                            DoctorId = doctorId,
                            Price = pricecheckOrRecheck,
                            Status = RequestType.Pending,
                            CouponId = couponId,
                            HourId = book.TimeId,
                        });
                    }
                    return true;
                }
            }
            return false;
        }
        public async Task<IQueryable<GetBookingForDoctorDto>> GetAllForDoctor(string userName, DateTime date)
        {
            string DoctorId = (await GetUserAsync(UserType.Doctor.ToString(), userName))?.Id;
            IQueryable<Booking> bookingDoctor = null;
            if (!date.Equals(DateTime.MinValue))
                bookingDoctor = _context.Bookings.Where(b => b.PatientId.Equals(DoctorId) && b.Date == date);
            else
                bookingDoctor = _context.Bookings.Where(b => b.PatientId.Equals(DoctorId));

            return bookingDoctor?.Select(b =>
            new GetBookingForDoctorDto()
            {
                Patientname = b.Doctor.User.UserName,
                Image = b.Doctor.User.Image,
                Day = b.Appointment.ExistingDay.ToString(),
                Hour = b.Hour.ExistHour,
                Status = b.Status.ToString(),
                BookingDate = b.Date.Value
            }).AsNoTracking();
        }
        public async Task<IQueryable<GetBookingForPatientDto>> GetAllForPatient(string userName)
        {
            string patientId = (await GetUserAsync(UserType.Patient.ToString(), userName))?.Id;
            return _context.Bookings.Where(b => b.PatientId.Equals(patientId)).Select(b =>
            new GetBookingForPatientDto()
            {
                DoctorName = b.Doctor.User.UserName,
                Image = b.Doctor.User.Image,
                Day = b.Appointment.ExistingDay.ToString(),
                Time = b.Hour.ExistHour.TotalHours,
                SpecializeName = b.Doctor.Specialization.Name,
                Status = b.Status.ToString(),
                BookingDate = b.Date,
                Price = b.Price,
                CouponId = b.CouponId.HasValue?b.CouponId.Value:0,/////////////////////////
                FinalPrice = _coupon.GetDiscountAsync(b.CouponId.Value, b.Price),
            }).AsNoTracking();
        }
        public IQueryable<GetBookingInfoDto> GetDoctorIdWithNumberOfRequests(int number,DateTime date)
        {
            IQueryable<Booking> booking = null;
            if (!date.Equals(DateTime.MinValue))
                booking = _context.Bookings.Where(b => b.Date >= date);
            else
                booking = _context.Bookings;

                 return  booking?.GroupBy(x => x.DoctorId).Select(x =>
                                                               new GetBookingInfoDto()
                                                               {
                                                                   DoctorId = x.Key,
                                                                   NumberOfRequests = x.Count(),
                                                               })
                                                               .OrderByDescending(x => x.NumberOfRequests)
                                                               .Take(number).AsNoTracking();
          
        }
        public IQueryable<dynamic> GetStatusWithRequestNumber(DateTime date)
        {
            IQueryable<Booking> booking = null;
            if (!date.Equals(DateTime.MinValue))
                booking = _context.Bookings.Where(x => x.Date >= date);
            else
                booking = _context.Bookings;
            return booking.GroupBy(x => x.Status)
                .Select(x =>
                new
                {
                    Status = x.Key.ToString(),
                    NumberOfRequests = x.Count(),
                }).AsNoTracking();
        }


    }
}
