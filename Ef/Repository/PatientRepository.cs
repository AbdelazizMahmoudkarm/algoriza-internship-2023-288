using algoriza_internship_288.Domain.AccountModels;
using algoriza_internship_288.Domain.Models;
using algoriza_internship_288.Domain.Models.Enums;
using Domain.DtoClasses.Patient;
using Microsoft.AspNetCore.Identity;
using Repository.IRepository;

namespace Repository.Repository
{
    public class PatientRepository : BaseRepository, IPatientRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientRepository(UserManager<ApplicationUser> usermanager, 
            SignInManager<ApplicationUser> signInManager) : base(usermanager, signInManager)
        =>
            _userManager = usermanager;
        public async Task<bool> AddAsync(AddPatientDto model)
        {
            if (model is null)
                return false;
            string userName = string.Concat(model.FName, model.LName);
                IdentityResult result = await _userManager.CreateAsync(new ApplicationUser()
                {
                    DateOfAdd= DateTime.Now,
                    Image = model.Image.ProcessImage(),
                    UserName =userName ,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    Gender =model.Gender, 
                    DateOfBirth=model.DateOfBirth,
                }, model.Password);
                if (result.Succeeded)
                {
                ApplicationUser user = GetUserByEmail(model.Email);
                    if (user is not null)
                    {
                        result = await _userManager.AddToRoleAsync(user, UserType.Patient.ToString());
                    if (result.Succeeded)
                        new Login { UserName = userName, Password = model.Password }.SendEmail(model.Email);
                    return result.Succeeded;
                    }
                }
            return false;
        }
        //private int GetAge(DateTime dob)
        //{
        //    int age = DateTime.Now.Year - dob.Year;
        //    if (DateTime.Now.DayOfYear < dob.DayOfYear)
        //        age -= 1;
        //    return age;
        //}
    }
}
