using algoriza_internship_288.Domain.Models;
using Domain.DtoClasses.Appointment;
using Domain.DtoClasses.Doctor;
using System.Linq.Expressions;

namespace Repository.Repository
{
    public  interface IDoctorRepository:IBaseRepository<Doctor>
    {
        public Task<bool> AddAsync(AddDoctorDto entity);
        public IQueryable<GetDoctorDto> GetByCondition(Expression<Func<Doctor, bool>> expression);
        public Task<bool> UpdateWithAppointmentAddAsync(AddDoctorAppointmentsDto appointmentModel, string userName);
        //public GetDoctorDto GetById(int id);
        public bool UpdateDoctor(UpdateDoctorDto editDoctor);
        public  Task<bool> Delete(int id, string name);
      //  public IQueryable<dynamic> GetDoctorIdWithNumberOfRequests(int number);
        public DoctorFilterIntoDto GetDoctorInfoWithSpecializeName(int doctorId);
        public Task<bool> UpdateAppointmentTimeAsync(EditAppointmentDto model, string userName);
        public Task<bool> DeleteAppointmentAsync(int hourId, string userName);
    }
}
