using algoriza_internship_288.Core.Models;
using algoriza_internship_288.Core.Models.Enums;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography;

namespace algoriza_internship_288.Ef.FluentApiClasses
{
    public static  class Relations 
    {
        public static void CreateDoctorRelation(this EntityTypeBuilder<Doctor> builder)
        {
            builder.HasOne(x=>x.User).
                    WithOne(x => x.Doctor)
                    .HasForeignKey<Doctor>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Specialization)
                    .WithMany(x => x.Doctors)
                    .HasForeignKey(x => x.SpecializeId)
                    .OnDelete(DeleteBehavior.Restrict);
        }

        public static void CreateApplicationUserRelation(this EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasIndex(x => x.Email).IsUnique(true);
            builder.HasIndex(x => x.UserName).IsUnique(true);
            
            builder.HasMany(x => x.Bookings)
                    .WithOne(x => x.Patient)
                    .HasForeignKey(x => x.PatientId);
        }
        public static void CreateCoupobRelation(this EntityTypeBuilder<Coupon> builder)
        {
            builder.HasMany(x=>x.Bookings)
                    .WithOne(x=>x.Coupon)
                    .HasForeignKey(x => x.CouponId)
                    .OnDelete(DeleteBehavior.SetNull);   
        }
        public static void CreateAppointMentRelation(this EntityTypeBuilder<Appointment> builder)
        {
            builder.HasOne<Doctor>()
                    .WithMany(x => x.Appointments)
                    .HasForeignKey(x => x.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Booking>()
                    .WithOne(x => x.Appointment)
                    .HasForeignKey<Booking>(x => x.AppointmentId).OnDelete(DeleteBehavior.Restrict);

        }
        public static void CreateAppointMentRelation(this EntityTypeBuilder<Time> builder)
        {
            builder.HasOne<Appointment>()
                 .WithMany(x => x.Times)
                 .HasForeignKey(x => x.AppointId)
                 .OnDelete(DeleteBehavior.Restrict);
        }
        public static void CreateBookingRelation(this EntityTypeBuilder<Booking> builder)
        {
            builder.Property(x => x.CouponId).IsRequired(false);
            builder.HasOne(x=>x.Doctor)
                   .WithMany(x => x.Requests)
                   .HasForeignKey(x => x.DoctorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x=>x.Patient)
                    .WithMany(x => x.Bookings)
                    .HasForeignKey(x => x.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
        public static void CreateHourRelation(this EntityTypeBuilder<Time> builder)
        {
            builder.HasOne<Booking>()
                   .WithOne(x => x.Hour)
                   .HasForeignKey<Booking>(x => x.HourId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
        public static void CreateSpecialization(this EntityTypeBuilder<Specialization> builder)
        =>  builder.HasIndex(x => x.Name).IsUnique(true);
        
        public static void CreateAdminUserWithAllRoles(this ModelBuilder builder)
        {
                string userId = Guid.NewGuid().ToString();
                string roleId = Guid.NewGuid().ToString();
            builder.Entity<ApplicationUser>().HasData(
                    new ApplicationUser()
                    {
                        Id = userId,
                        UserName = "Abdelaziz",
                        Email = "Abdelaziz.2023@gmail.com",
                        PasswordHash = HashedPassword("Abdelaziz_mk@123"),
                        DateOfBirth = DateTime.Now.AddYears(-35),
                        Gender = Gender.Male,
                        DateOfAdd = DateTime.Now
                    });
            string admin = nameof(UserType.Admin);
            string doctor = nameof(UserType.Doctor);
            string patient = nameof(UserType.Patient);
            builder.Entity<IdentityRole>().HasData(
                   new IdentityRole()
                   {
                       Id = roleId,
                       Name = admin,
                       NormalizedName= admin.ToUpper()
                   },
                   new IdentityRole()
                   {
                       Name = doctor,
                       NormalizedName = doctor.ToUpper()
                   },
                   new IdentityRole()
                   {
                       Name =patient,
                       NormalizedName=patient.ToUpper()
                   });

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>()
                {
                    RoleId = roleId,
                    UserId = userId,
                });
        }
        private static string HashedPassword(string password)
        {
            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[128 / 8];
            using var rand =  RandomNumberGenerator.Create(); 
                rand.GetNonZeroBytes(salt);
            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            return  Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                                password: password,
                                                                salt: salt,
                                                                prf: KeyDerivationPrf.HMACSHA256,
                                                                iterationCount: 100000,
                                                                numBytesRequested: 256 / 8));
        }

    }
}
