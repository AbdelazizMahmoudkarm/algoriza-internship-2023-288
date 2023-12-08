using algoriza_internship_288.Domain.Models.Enums;
using algoriza_internship_288.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain.FluentApiClasses
{
    public static class InsertAdmin
    {
        public static async Task CreateAdminAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                string email = "abdelazizmahmoukarm@gmail.com";
                IdentityResult result = await userManager.CreateAsync(new ApplicationUser()
                {
                    DateOfAdd = DateTime.Now,
                    UserName = "AbdelazizMahmoud",
                    Email = email,
                    PhoneNumber = "01021050450",
                    Gender = Gender.Male,
                }, "Koko_Mahmoud.123");
                if (result.Succeeded)
                {
                    ApplicationUser user = await userManager.FindByEmailAsync(email);
                    if (user is not null)
                    {
                        result = await userManager.AddToRoleAsync(user, UserType.Admin.ToString());
                    }
                }
            }
                //    var appUserStore = new UserStore<ApplicationUser>(context);
                //    var appUserManager = new UserManager<ApplicationUser>(appUserStore,AppDbcontext);
                //    var appRoleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context));


                //    string userId = Guid.NewGuid().ToString();
                //    string roleId = Guid.NewGuid().ToString();
                //    string username = "AbdelazizMahmoud";
                //    string email = "AbdelazizMahmouKarm@gmail.com";
                //    builder.Entity<ApplicationUser>().HasData(
                //            new ApplicationUser()
                //            {
                //                Id = userId,
                //                UserName = username,
                //                NormalizedEmail=username.ToUpper(),
                //                Email = email,
                //                NormalizedUserName=email.ToUpper(),
                //                PasswordHash = "AQAAAAIAAYagAAAAEIS4W6Ujy8w4w1rIVnKIeExXbsj65Gp6QeocW43Mn8jSh1dP6XH8Af8+IsuFq3rczw==", 
                //                SecurityStamp= "2TSW2IJTQDTD43XIVPNN35HMWIHEL2HW",
                //                ConcurrencyStamp= "4bd50945-a20a-48a4-9eaa-b9f3abef2f26",
                //                PhoneNumber= "01021050450",
                //                //HashedPassword("Abdelaziz_mk@123"),
                //                DateOfBirth = DateTime.Now.AddYears(-35),
                //                Gender = Gender.Male,
                //                DateOfAdd = DateTime.Now
                //            });
                //    string admin = nameof(UserType.Admin);
                //    string doctor = nameof(UserType.Doctor);
                //    string patient = nameof(UserType.Patient);
                //    builder.Entity<IdentityRole>().HasData(
                //           new IdentityRole()
                //           {
                //               Id = roleId,
                //               Name = admin,
                //               NormalizedName = admin.ToUpper()
                //           },
                //           new IdentityRole()
                //           {
                //               Name = doctor,
                //               NormalizedName = doctor.ToUpper()
                //           },
                //           new IdentityRole()
                //           {
                //               Name = patient,
                //               NormalizedName = patient.ToUpper()
                //           });

                //    builder.Entity<IdentityUserRole<string>>().HasData(
                //        new IdentityUserRole<string>()
                //        {
                //            RoleId = roleId,
                //            UserId = userId,
                //        });
                //}
                //private static string HashedPassword(string password)
                //{
                //    //byte[] salt;
                //    //byte[] buffer2;

                //    //using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
                //    //{
                //    //    salt = bytes.Salt;
                //    //    buffer2 = bytes.GetBytes(0x20);
                //    //}
                //    //byte[] dst = new byte[0x31];
                //    //Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
                //    //Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
                //    //return Convert.ToBase64String(dst);

                //    byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); ;
                //    using var rand = RandomNumberGenerator.Create();
                //    return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                //                                         password: password!,
                //                                         salt: salt,
                //                                         prf: KeyDerivationPrf.HMACSHA256,
                //                                         iterationCount: 100000,
                //                                         numBytesRequested: 256 / 8));
                //}

                // }
        }
    }
}
