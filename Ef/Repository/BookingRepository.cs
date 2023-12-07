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
        public static bool _arabic;
        public BookingRepository(AppDbContext context, UserManager<ApplicationUser> userManager,
            ICouponRepository coupon,IAppointmentRepository appointment,
            ITimeRepository time,SignInManager<ApplicationUser> signInManager,bool arabic) 
            : base(userManager, signInManager)
        {
            _context = context;
            _coupon = coupon;
            _appointment = appointment;
            _time = time;
            _userManager= userManager;
            _arabic= arabic;
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
                    if (/*book.AppointmentId != 0 &&*/ book.TimeId != 0)
                    {
                        //int appointmentId = appointment.AppointmentId;
                        //int hourId = appointment.TimeId;

                        bool checkIfTimeExistBefore = _context.Bookings
                            .Any(x => x.DoctorId == doctorId&&  x.HourId == book.TimeId);
                        if (checkIfTimeExistBefore)
                            return false;
                        bool bookingSameDoctorWithSameDay = _context.Bookings
                            .Any(x => x.DoctorId == doctorId &&x.PatientId==patientId &&x.Date.Value.DayOfYear == DateTime.Now.DayOfYear);
                        if (bookingSameDoctorWithSameDay)
                            return false;
                        if(!_time.CheckIfTimeExistForDoctor(book.TimeId , doctorId))
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

                            if (coupon != null)
                            {
                                if (_context.Bookings
                                        .Count(booking => booking.PatientId == patientId
                                            && booking.CouponId.Equals(null)
                                            && booking.Status == RequestType.Completed)
                                                     >= coupon.NumberOfRequestCompleted)
                                    couponId = coupon.Id;
                            }
                        }
                        _context.Bookings.Add(new Booking()
                        {
                            PatientId = patientId,
                            Date = DateTime.Now,
                           // AppointmentId = book.AppointmentId,
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
            int? DoctorId = (await GetUserAsync(UserType.Doctor.ToString(), userName)).Doctor?.Id;
            if(!DoctorId.HasValue)
                return null;
            IQueryable<Booking> bookingDoctor;
            if (!date.Equals(DateTime.MinValue))
                bookingDoctor = _context.Bookings.Where(b => b.DoctorId.Equals(DoctorId) && b.Date >= date);
            else
                bookingDoctor = _context.Bookings.Where(b => b.DoctorId.Equals(DoctorId));

            return bookingDoctor.Select(b =>
            new GetBookingForDoctorDto()
            {
                Patientname = b.Patient.UserName,
                Gender=b.Patient.Gender.GetGender(_arabic),
                Image = b.Patient.Image,
                Email= b.Patient.Email,
                age=GetAge(b.Patient.DateOfBirth),
                Phone=b.Patient.PhoneNumber,
                Day = b.Hour.Appointment.ExistingDay.GetDay(_arabic),
                Hour = b.Hour.ExistHour,
                Status = b.Status.GetStatus(_arabic),
                BookingDate = b.Date.Value
            }).AsNoTracking();
        }
        private static int GetAge(DateTime dob)
        {
            int age = DateTime.Now.Year - dob.Year;
            if (DateTime.Now.DayOfYear < dob.DayOfYear)
                age -= 1;
            return age;
        }
        public async Task<IEnumerable<GetBookingForPatientDto>> GetAllForPatient(string userName)
        {
            string patientId = (await GetUserAsync(UserType.Patient.ToString(), userName))?.Id;
           

           var patientbooking= _context.Bookings.Where(b => b.PatientId.Equals(patientId)).AsEnumerable().Select( b =>
            new GetBookingForPatientDto()
            {
                DoctorName = b.Doctor.User.UserName,
                SpecializeName = _arabic ? b.Doctor.Specialization.ArName : b.Doctor.Specialization.Name,
                DoctorImage = b.Doctor.User.Image,
                Day = b.Hour.Appointment.ExistingDay.GetDay(_arabic),
                Time = b.Hour.ExistHour.TotalHours,
                Status = b.Status.GetStatus(_arabic),
                BookingDate = b.Date,
                Price = b.Price,
                DiscountCode = b.CouponId.HasValue ? b.Coupon.Code:_arabic? "لا يوجد":" No Thing" ,/////////////////////////
               FinalPrice = b.CouponId.HasValue? _coupon.GetDiscount(b.CouponId.Value, b.Price):b.Price,
            });
            return patientbooking;
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
