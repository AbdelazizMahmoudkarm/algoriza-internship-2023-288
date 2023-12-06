using algoriza_internship_288.Domain.Models;
using algoriza_internship_288.Domain.Models.Enums;
using algoriza_internship_288.Repository.DAL;
using Domain.DtoClasses.Appointment;
using Domain.DtoClasses.Doctor;
using Microsoft.AspNetCore.Identity;
using Repository.IRepository;
using System.Linq.Expressions;

namespace Repository.Repository
{
    public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IAppointmentRepository _appointment;
        private readonly ISpecializationRepository _specialize;
        private readonly IBookingRepository _booking;

        public DoctorRepository(UserManager<ApplicationUser> userManager, AppDbContext context,
            IAppointmentRepository appointment, SignInManager<ApplicationUser> signInManager,
            ISpecializationRepository specialize,IBookingRepository booking) : base(userManager, signInManager)
        {
            _userManager = userManager;
            _context = context;
            _appointment = appointment;
            _specialize = specialize;
            _booking = booking;
        }

        public async Task<bool> AddAsync(AddDoctorDto model)
        {
            if (model is not null)
            {
                Doctor doctor = new();
                if (model.SpecializeId != 0)
                    doctor.SpecializeId = model.SpecializeId;
                IdentityResult result = await _userManager.CreateAsync(new ApplicationUser()
                {
                    DateOfAdd = DateTime.Now,
                    Image = ProcessImage(model.Image),
                    UserName = string.Concat(model.FName, model.LName),
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    Gender = model.Gender,
                    Doctor = doctor
                }, model.Password);
                if (result.Succeeded)
                {
                    ApplicationUser user = GetUserByEmail(model.Email);
                    if (user is not null)
                    {
                        result = await _userManager.AddToRoleAsync(user, UserType.Doctor.ToString());
                        return result.Succeeded;
                    }
                }
                return result.Succeeded;
            }
            return false;
        }
           
        public async Task<bool> UpdateWithAppointmentAddAsync(AddDoctorAppointmentsDto appointmentModel, string userName)
        {
            bool isOk = false;
            ApplicationUser userDoctor = await GetUserAsync(UserType.Doctor.ToString(), userName);
            if (userDoctor is not null)
            {
                userDoctor.Doctor.CheckPrice = appointmentModel.CheckPrice;
                userDoctor.Doctor.ReCheckPrice = appointmentModel.ReCheckPrice;
                userDoctor.Doctor.Appointments = _appointment.AddAppointments(appointmentModel.Appointments,userDoctor.Doctor.Id);
                
                await _userManager.UpdateAsync(userDoctor);
                isOk = true;
            }
            return isOk;
        }
        public async Task<bool> UpdateAppointmentTimeAsync(EditAppointmentDto model,string userName)
        {
            int doctorId = (await GetUserAsync(UserType.Doctor.ToString(), userName)).Doctor.Id;
            if(doctorId !=0)
            {
                if(!_booking.CheckIfDayAndTimeExists(doctorId, model.hourId))
                    return _appointment.UpdateAppointment(model);
            }
            return false;
        }
        public async Task<bool> DeleteAppointmentAsync(int hourId, string userName)
        {
            int doctorId = (await GetUserAsync(UserType.Doctor.ToString(), userName)).Doctor.Id;
            if (doctorId != 0)
            {
                if (!_booking.CheckIfDayAndTimeExists(doctorId, hourId))
                    return _appointment.DeleteAppointment(hourId);
            }
            return false;
        }
        public bool UpdateDoctor(UpdateDoctorDto doctorModel)
        {
            Doctor doctor = _context.Doctors.Where(x => x.Id == doctorModel.Id).FirstOrDefault();
            if (doctor != null)
            {
                doctor.Id = doctorModel.Id;
                doctor.CheckPrice = doctorModel.CheckPrice;
                doctor.ReCheckPrice = doctorModel.ReCheckPrice;
                doctor.SpecializeId = doctorModel.SpecializeId;
                doctor.User.UserName = string.Concat(doctorModel.FName, doctorModel.LName);
                doctor.User.Gender = doctorModel.Gender;
                doctor.User.Email = doctorModel.Email;
                doctor.User.Image = doctorModel.Image;
                doctor.User.DateOfBirth = doctorModel.DateOfBirth;
                doctor.User.PhoneNumber = doctorModel.Phone;
            }
           
            //int specializeid = _specialize.GetByName(doctorModel.SpecialzeName);
            //if (specializeid != 0)
            //    doctor.SpecializeId = specializeid;
            //else
            //    doctor.Specialization = _specialize.AddByName(doctorModel.SpecialzeName);
            _context.Update(doctor);
            return true;
        }
        public async Task<bool> Delete(int id,string name)
        {
           dynamic doctorIdWithBookings = _context.Doctors.Where(x => x.Id.Equals(id)).Select(x =>
           new
           {
               BookingCount= x.User.Bookings.Count,
               x.Id
           }).FirstOrDefault();
            if (doctorIdWithBookings != null)
            {
                if (doctorIdWithBookings.BookingCount == 0)
                {
                    IdentityResult result = await _userManager.DeleteAsync(new() { Id = doctorIdWithBookings.Id });
                    return result.Succeeded;
                }
            }
            return false;
        }

        ///ToDo
        public IQueryable<GetDoctorDto> GetByCondition(Expression<Func<Doctor, bool>> condition)
        {
            IQueryable<Doctor> doctor = null;
            if (condition is not null)
                doctor = _context.Doctors.Where(condition);
            else
                doctor = _context.Doctors;
            return doctor.Select(x => new GetDoctorDto()
            {
                Id = x.Id,
                UserName = x.User.UserName,
                SpecializeNmae = x.Specialization.Name,
                Email = x.User.Email,
                Gender = x.User.Gender.ToString(),
                CheckPrice = x.CheckPrice,
                ReCheckPrice = x.ReCheckPrice,
                Appointments = _appointment.GetDaysAndTimes(x.Id).ToList()
            });
        }
       

        public DoctorFilterIntoDto GetDoctorInfoWithSpecializeName(int doctorId)
        {
            return _context.Doctors
                          .Where(x => x.Id.Equals(doctorId))
                          .Select(x =>
                          new DoctorFilterIntoDto
                          {
                              Image = x.User.Image,
                              FullName = x.User.UserName,
                              SpecializeName = x.Specialization.Name
                          }).FirstOrDefault();
        }        
    }
}
