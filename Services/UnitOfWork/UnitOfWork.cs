using algoriza_internship_288.Domain.Models;
using algoriza_internship_288.Repository.DAL;
using Domain.FluentApiClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Repository.IRepository;
using Repository.Repository;

namespace Service.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private IDbContextTransaction _transaction;
        public ICouponRepository Coupon { get; private set;}
        public IPatientRepository Patient { get; private set;}
        public IAppointmentRepository Appointment { get; private set;}
        public ISpecializationRepository Specialization { get; private set; }
        public ITimeRepository Time { get; private set; }
        public IDoctorRepository Doctor { get; private set; }

        public IBookingRepository Booking { get; private set; }
       

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool _disposed = false;

        public UnitOfWork(AppDbContext context, UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
        {
            Task.Run(() => InsertAdmin.CreateAdminAsync(userManager)).Wait();
            _context = context;
            _userManager = userManager;
            
            Specialization= new SpecializationRepository(context);
            Patient= new PatientRepository(context, userManager, signInManager);
            Time= new TimeRepository(context);
            Appointment=new AppointmentRepository(context,Time);
            Booking =new BookingRepository(_context, _userManager, Coupon, Appointment, Time,signInManager);
            Doctor = new DoctorRepository(userManager, context, Appointment, signInManager, Specialization, Booking);
            Coupon = new CouponRepository(context, Booking);
        }

        public virtual void Dispose(bool dispose)
        {
            if (!_disposed)
                if (dispose)
                    _context.Dispose();
            _disposed = true;
        }


        public void RollBack()
            => _transaction?.Rollback();


        public Task<int> SaveAsync()
        {
            try
            {
                return _context.SaveChangesAsync();
            }
            catch (Exception dbEx)
            {
                return Task.Run(() => 0);
            }
        }
    }
}
