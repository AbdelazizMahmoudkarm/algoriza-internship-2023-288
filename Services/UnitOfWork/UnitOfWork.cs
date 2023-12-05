using algoriza_internship_288.Core.Models;
using algoriza_internship_288.Ef.DAL;
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
        public ICouponRepository Coupon
        {
            // get => _coupon ??= new CouponRepository(_context, _userManager);
            get; private set;
        }
        public IPatientRepository Patient
        {
            // get => _patient ??= new PatientRepository(_context, _userManager);
            get; private set;
        }
        
        public IAppointmentRepository Appointment
        {
            // get => _appointment ??= new AppointmentRepository(_context, _userManager, _signInManager, _time);
            get; private set;
        }
        public ISpecializationRepository Specialization
        {
            // get => _specialization ??= new SpecializationRepository(_context, _userManager);
            get; private set;
        }
        public ITimeRepository Time
        {
            // get => _time ??= new TimeRepository(_context, _userManager);
            get; private set;
        }
        public IDoctorRepository Doctor
        {
            //  get => _doctor ??= new DoctorRepository(_userManager, _context, _appointment, _signInManager, _specialization);
            get; private set;
        }

        public IBookingRepository Booking
        {
            // get => _booking ??= new BookingRepository(_context, _userManager, _coupon, _appointment, _time);
            get; private set;
        }
        //public Task<int> CommitTransactionAsync()
        //{
        //   return  SaveAsync();
        //}

       // public void CreateTransaction()
       //=> _transaction ??= _context.Database.BeginTransaction();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool _disposed = false;

        public UnitOfWork(AppDbContext context, UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
        {
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
