﻿using algoriza_internship_288.Core.Models;
using algoriza_internship_288.Core.Models.Enums;
using algoriza_internship_288.Ef.DAL;
using Domain.DtoClasses.Patient;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository.Repository
{
    public class PatientRepository : BaseRepository<ApplicationUser>, IPatientRepository
    {
        //private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        //private readonly IBookingRepository _booking;

        public PatientRepository(AppDbContext context, UserManager<ApplicationUser> usermanager, 
            SignInManager<ApplicationUser> signInManager) : base(usermanager, signInManager)
        {
            //_context = context;
            _userManager = usermanager;
            
        }
        public async Task<bool> AddAsync(AddPatientDto model)
        {
                IdentityResult result = await _userManager.CreateAsync(new ApplicationUser()
                {
                    DateOfAdd= DateTime.Now,
                    Image = model.Image,//ProcessImage(doctor.Image),
                    UserName = string.Concat(model.FName, model.LName),
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