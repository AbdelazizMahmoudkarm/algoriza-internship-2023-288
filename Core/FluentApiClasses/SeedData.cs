using algoriza_internship_288.Domain.Models.Enums;
using algoriza_internship_288.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Domain.FluentApiClasses
{
    public static class SeedData
    {
        public static void CreateAllRoles(this ModelBuilder builder)
        {
            #region commented
            //string userId = Guid.NewGuid().ToString();
            //string roleId = Guid.NewGuid().ToString();
            //string username = "AbdelazizMahmoud";
            //string email = "AbdelazizMahmouKarm@gmail.com";
            //builder.Entity<ApplicationUser>().HasData(
            //        new ApplicationUser()
            //        {
            //            Id = userId,
            //            UserName = username,
            //            NormalizedEmail=username.ToUpper(),
            //            Email = email,
            //            NormalizedUserName=email.ToUpper(),
            //            PasswordHash = "AQAAAAIAAYagAAAAEIS4W6Ujy8w4w1rIVnKIeExXbsj65Gp6QeocW43Mn8jSh1dP6XH8Af8+IsuFq3rczw==", 
            //            SecurityStamp= "2TSW2IJTQDTD43XIVPNN35HMWIHEL2HW",
            //            ConcurrencyStamp= "4bd50945-a20a-48a4-9eaa-b9f3abef2f26",
            //            PhoneNumber= "01021050450",
            //            //HashedPassword("Abdelaziz_mk@123"),
            //            DateOfBirth = DateTime.Now.AddYears(-35),
            //            Gender = Gender.Male,
            //            DateOfAdd = DateTime.Now
            //        });

            #endregion

            string admin = nameof(UserType.Admin);
            string doctor = nameof(UserType.Doctor);
            string patient = nameof(UserType.Patient);
            builder.Entity<IdentityRole>().HasData(
                   new IdentityRole()
                   {
                       Name = admin,
                       NormalizedName = admin.ToUpper()
                   },
                   new IdentityRole()
                   {
                       Name = doctor,
                       NormalizedName = doctor.ToUpper()
                   },
                   new IdentityRole()
                   {
                       Name = patient,
                       NormalizedName = patient.ToUpper()
                   });
        }


        public static void CreateSpecialization(this ModelBuilder builder)
        {
            builder.Entity<Specialization>().HasData(
                new Specialization()
                {
                    Id=1,
                    ArName="عيون",
                    Name="Eyes"
                },
                new Specialization()
                {
                    Id=2,
                    ArName="جلدية",
                    Name="Skin"
                },new Specialization()
                {
                    Id=3,
                    ArName="عظام",
                    Name="Bones"
                },new()
                {
                    Id=4,
                    ArName="اسنان",
                    Name="Teeth"
                });
        }

    }

}
