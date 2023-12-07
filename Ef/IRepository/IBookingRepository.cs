using algoriza_internship_288.Domain.Models;
using Domain.DtoClasses.Booking;
using Repository.Repository;

namespace Repository.IRepository
{
    public  interface IBookingRepository:IBaseRepository<Booking>
    {
        public Task<bool> AddAsync(AddBookingDto book, string userName);
        public IQueryable<dynamic> GetStatusWithRequestNumber(DateTime date);
        public  Task<IEnumerable<GetBookingForPatientDto>> GetAllForPatient(string userName);
        public Task<bool> CancelAsync(int bookingId,string userName);
        public IQueryable<GetBookingInfoDto> GetDoctorIdWithNumberOfRequests(int number,DateTime date);
        public  Task<bool> ConfirmAsync(int bookingId, string userName);
        public  Task<bool> CheckIfCouponExistBookingAsync(int couponId);
        public Task<IQueryable<GetBookingForDoctorDto>> GetAllForDoctor(string userName,DateTime date);
        public bool CheckIfDayAndTimeExists(int doctorId, int timeId);
    }
}
