using algoriza_internship_288.Domain.AccountModels;
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
    public class DoctorRepository : BaseRepository, IDoctorRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IAppointmentRepository _appointment;
        
        private readonly IBookingRepository _booking;
        private static bool _arabic;

        public DoctorRepository(UserManager<ApplicationUser> userManager, AppDbContext context,
            IAppointmentRepository appointment, SignInManager<ApplicationUser> signInManager,
            IBookingRepository booking, bool arabic) : base(userManager, signInManager)
        {
            _userManager = userManager;
            _context = context;
            _appointment = appointment;
            
            _booking = booking;
            _arabic = arabic;
        }

        public async Task<bool> AddAsync(AddDoctorDto model)
        {
            if (model is not null)
            {
                Doctor doctor = new();
                if (model.SpecializeId != 0)
                    doctor.SpecializeId = model.SpecializeId;
                string userName = string.Concat(model.FName, model.LName);
                IdentityResult result = await _userManager.CreateAsync(new ApplicationUser()
                {
                    DateOfAdd = DateTime.Now,
                    Image = model.Image.ProcessImage(),
                    UserName = userName,
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
                        if (result.Succeeded)
                            new Login { UserName = userName, Password = model.Password }.SendEmail(model.Email);
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
        public async Task<bool> UpdateAppointmentTimeAsync(UpdateAppointmentDto model,string userName)
        {
            int? doctorId = (await GetUserAsync(UserType.Doctor.ToString(), userName))?.Doctor.Id;
            if(doctorId.HasValue)
            {
                if(!_booking.CheckIfDayAndTimeExists(doctorId.Value, model.HourId))
                    return _appointment.UpdateAppointment(model);
            }
            return false;
        }
        public async Task<bool> DeleteAppointmentAsync(int hourId, string userName)
        {
            int? doctorId = (await GetUserAsync(UserType.Doctor.ToString(), userName))?.Doctor.Id;
            if (doctorId.HasValue)
            {
                if (!_booking.CheckIfDayAndTimeExists(doctorId.Value, hourId))
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
                doctor.User.Image = doctorModel.Image.ProcessImage();
                doctor.User.DateOfBirth = doctorModel.DateOfBirth;
                doctor.User.PhoneNumber = doctorModel.Phone;
            }
            _context.Update(doctor);
            return true;
        }
        public async Task<bool> Delete(int id,string name)
        {
            if (_booking.CheckDoctorRequests(id))
                return false;
            string doctorId = _context.Doctors.FirstOrDefault(x => x.Id == id)?.UserId;
            IdentityResult result = await _userManager.DeleteAsync(new() { Id = doctorId });
            return result.Succeeded;
        }

        ///ToDo
        public IQueryable<GetDoctorDto> GetByCondition(Expression<Func<Doctor, bool>> condition)
        {
            IQueryable<Doctor> doctor ;
            if (condition is not null)
                doctor = _context.Doctors.Where(condition);
            else
                doctor = _context.Doctors;
            return doctor.Select(x => new GetDoctorDto()
            {
                Id = x.Id,
                UserName = x.User.UserName,
                SpecializeNmae =_arabic ? x.Specialization.ArName : x.Specialization.Name,
                Email = x.User.Email,
                Gender = x.User.Gender.GetGender(_arabic),
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
                              SpecializeName = _arabic ?x.Specialization.ArName : x.Specialization.Name,
                          }).FirstOrDefault();
        }        
    }
}
