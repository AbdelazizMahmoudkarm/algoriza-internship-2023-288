using Repository.IRepository;
using Repository.Repository;

namespace Service.UnitOfWork
{
    public  interface IUnitOfWork:IBaseRepository,IDisposable
    {
        public IDoctorRepository Doctor { get; }
        public IPatientRepository Patient { get; }
        public IBookingRepository Booking { get; }
        public ICouponRepository Coupon { get; }
        public IAppointmentRepository Appointment { get; }
        public ISpecializationRepository Specialization { get; }

        public ITimeRepository Time { get; }
        //public void CreateTransaction();
        //public Task<int> CommitTransactionAsync();
        public Task<int> SaveAsync();
        //public void RollBack();
    }
}
