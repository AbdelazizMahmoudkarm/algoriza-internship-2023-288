using algoriza_internship_288.Core.Models;
using Domain.DtoClasses.Appointment;

namespace Repository.IRepository
{
    public interface ITimeRepository
    {
        public int? GetBy(double time,int appointmentId);
        public List<Time> Add(List<double> timesModel,int appointmentId);
        public int? CheckTime(int appointmentId, double time);
        public bool Update(EditAppointmentDto model);
       public  bool Delete(int hourId);
    }
}
