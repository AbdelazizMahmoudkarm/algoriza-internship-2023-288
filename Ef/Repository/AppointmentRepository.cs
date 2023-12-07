using algoriza_internship_288.Domain.Models;
using algoriza_internship_288.Domain.Models.Enums;
using algoriza_internship_288.Repository.DAL;
using Domain.DtoClasses;
using Domain.DtoClasses.Appointment;
using Repository.IRepository;

namespace Repository.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;
        private readonly ITimeRepository _time;
        public static  bool _arabic;
        public AppointmentRepository(AppDbContext context, ITimeRepository time,bool arabic) 
            
        {
            _context = context;
            _time=time;
            _arabic=arabic;
        }
        //public int? GetBy(Days day,int doctorId)
        //{
        // return  _context.Appointments
        //        .Where(x => x.DoctorId.Equals(doctorId) &&
        //        x.ExistingDay
        //        .Equals(day))
        //        .FirstOrDefault()?.Id;
        //}

        //public bool CheckIfAppointmentIsExsist(int appointmentId,int timeId)
        //=> _context.Appointments.Any(x => x.Id == appointmentId && x.Times.Any(x => x.Id == timeId));

        public bool UpdateAppointment(EditAppointmentDto appointment)
        {
            bool result = _time.Update(appointment);
            return result;
        }
        public bool DeleteAppointment(int hourId)
        {
            bool result = _time.Delete(hourId);
            return result;
        }
        public  List<Appointment> AddAppointments(List<AddAppointmentDto> model, int doctorId)
        {
            if (model is not null && model.Any())
            {
                List<Appointment> appointments = new();
                foreach (AddAppointmentDto day in model)
                {
                    int appointmentId = _context.Appointments//check if day and time is exist or not
                        .Where(x => x.DoctorId == doctorId && x.ExistingDay.Equals(day.Days))
                        .Select(x => x.Id).FirstOrDefault();

                    List<Time> times = _time.Add(day.Times, appointmentId);
                    if (appointmentId != 0)
                    {
                        if (times is not null && times.Any())
                        {
                            appointments.Add(new()
                            {
                                Id = appointmentId,
                                ExistingDay = day.Days,
                                Times = times,
                            });
                        }
                    }
                    else
                    {
                        appointments.Add(new()
                        {
                            ExistingDay = day.Days,
                            Times = times,
                        });
                    }
                  
                }
                return appointments;
            }
            return default;
        }
        public dynamic GetAppointmentIdWithTimeIdOrDefault(int doctorId, Days day,double time)
        {
            int appointmentid = _context.Appointments
                .Where(x => x.DoctorId == doctorId && x.ExistingDay.Equals(day))
                .Select(x => x.Id).FirstOrDefault();
            int? timeId =_time.CheckTime(appointmentid, time);
            if (appointmentid != 0 && timeId is not null )
            {
                return new
                {
                    AppointmentId = appointmentid,
                    TimeId = timeId,
                };
            }
            return default;
        }
        public IQueryable<GetAppointmentDto> GetDaysAndTimes(int doctorid)
        {
            return _context.Appointments
                .Where(x => x.DoctorId.Equals(doctorid))
                .Select(appointmrnt => new GetAppointmentDto()
                {
                    Days = appointmrnt.ExistingDay.GetDay(_arabic),
                    Times = appointmrnt.Times.Select(x => x.ExistHour).ToList(),
                });
        }
     
    }
}
