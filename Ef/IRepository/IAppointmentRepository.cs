using algoriza_internship_288.Domain.Models;
using algoriza_internship_288.Domain.Models.Enums;
using Domain.DtoClasses.Appointment;
using Repository.Repository;

namespace Repository.IRepository
{
    public  interface IAppointmentRepository
    {
        // public int? GetBy(Days day,int doctorId);
        public List<Appointment> AddAppointments(List<AddAppointmentDto> model, int doctorId);
        //public bool CheckIfAppointmentIsExsist(int appointmentId, int timeId);
        public IQueryable<GetAppointmentDto> GetDaysAndTimes(int doctorid);
        public dynamic GetAppointmentIdWithTimeIdOrDefault(int doctorId, Days day, double time);
        public bool UpdateAppointment(UpdateAppointmentDto appointment);
        public bool DeleteAppointment(int hourId);

    }
}
