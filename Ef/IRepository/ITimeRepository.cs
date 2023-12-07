using algoriza_internship_288.Domain.Models;
using Domain.DtoClasses.Appointment;

namespace Repository.IRepository
{
    public interface ITimeRepository
    {
        //public int? GetBy(double time,int appointmentId);
        public bool CheckIfTimeExistForDoctor(int timeId, int doctorId);
        public List<Time> Add(List<double> timesModel,int appointmentId);
        public int? CheckTime(int appointmentId, double time);
        public bool Update(EditAppointmentDto model);
       public  bool Delete(int hourId);
    }
}
