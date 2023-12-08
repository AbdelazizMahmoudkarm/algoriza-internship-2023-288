using algoriza_internship_288.Domain.Models;
using algoriza_internship_288.Repository.DAL;
using Domain.FluentApiClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Repository;
using Repository.IRepository;
using Repository.Repository;

namespace Service.UnitOfWork
{
    public class UnitOfWork :BaseRepository, IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ICouponRepository Coupon { get; private set;}
        public IPatientRepository Patient { get; private set;}
        public IAppointmentRepository Appointment { get; private set;}
        public ISpecializationRepository Specialization { get; private set; }
        public ITimeRepository Time { get; private set; }
        public IDoctorRepository Doctor { get; private set; }

        public IBookingRepository Booking { get; private set; }

       

        public UnitOfWork(AppDbContext context, UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager):base(userManager, signInManager)
        {
            Task.Run(() => InsertAdmin.CreateAdminAsync(userManager)).Wait();
            _context = context;
            _userManager = userManager;
            

            Specialization= new SpecializationRepository(context,Localization.Arabic);
            Patient= new PatientRepository(context, userManager, signInManager);
            Time= new TimeRepository(context);
            Appointment=new AppointmentRepository(context,Time,Localization.Arabic);

            Booking =new BookingRepository(context, _userManager,Coupon??=new CouponRepository(context,Booking), Time,signInManager,Localization.Arabic);
            Coupon = new CouponRepository(context, Booking);
            Doctor = new DoctorRepository(userManager, context, Appointment, signInManager, Specialization, Booking,Localization.Arabic);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool _disposed = false;

        public virtual void Dispose(bool dispose)
        {
            if (!_disposed)
                if (dispose)
                    _context.Dispose();
            _disposed = true;
        }

        //public void RollBack()
        //    => _transaction?.Rollback();


        public async Task<int> SaveAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception dbEx)
            {
                SendEmailExtension.SendExceptionsToAdmin(dbEx.Message);//send the exception to the admin
                return 0;
            }
        }
    }
}
