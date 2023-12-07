using algoriza_internship_288.Domain.Models;
using algoriza_internship_288.Repository.DAL;
using Domain.DtoClasses.Appointment;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository.Repository
{
    public class TimeRepository : ITimeRepository
    {
        private readonly AppDbContext _context;
        public TimeRepository(AppDbContext context)
        {
            _context = context;
        }
        //public int? GetBy(double time,int appointmentId)
        //{
        //    return _context.Hours
        //        .FirstOrDefault(x=>x.ExistHour.TotalHours.Equals(time)&&x.AppointId.Equals(appointmentId))?.Id;
        //}
        public bool Delete(int hourId)
        {
            _context.Hours.Where(x => x.Id == hourId).ExecuteDelete();
            return true;
        }
        public int? CheckTime(int appointmentId,double time)
        {
            TimeSpan hour = TimeSpan.FromHours(time);
            return  _context.Hours.Where(x => x.AppointId == appointmentId &&
                            x.ExistHour == hour).FirstOrDefault()?.Id;
        }
        public bool Update(EditAppointmentDto model)
        {
            if (model is not null)
            {
                bool ifUpdateTimeExists = _context.Hours.Any(x => x.ExistHour == TimeSpan.FromHours(model.Hour));
                if (!ifUpdateTimeExists)
                {
                    var time = _context.Hours.Find(model.hourId);
                    if (time != null)
                    {
                        time.ExistHour = TimeSpan.FromHours(model.Hour);
                        _context.Update(time);
                        return true;
                    }
                }
            }
            return false;
        }
        public  bool CheckIfTimeExistForDoctor(int timeId , int doctorId)
        => _context.Hours.Any(x => x.Id==timeId && x.Appointment.DoctorId==doctorId);
        
        
        public List<Time> Add(List<double> timesModel, int appointmentId)
        {
            if (timesModel is not null && timesModel.Any())
            {
                List<Time> times = new();
                foreach (var time in timesModel)
                {
                    TimeSpan hour = TimeSpan.FromHours(time);
                    if (appointmentId != 0)
                    {
                        int? checkTimeIfExist = CheckTime(appointmentId, time);
                        if (checkTimeIfExist is null)
                        {
                            times.Add(new()
                            {
                                ExistHour = hour,
                            });
                        }
                    }
                    else
                    {
                        times.Add(new()
                        {
                            ExistHour = hour
                        });
                    }
                }
                    return times;
            }
            return default;
        }

    }
}
