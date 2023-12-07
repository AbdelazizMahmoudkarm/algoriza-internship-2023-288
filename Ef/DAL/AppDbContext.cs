using algoriza_internship_288.Domain.Models;
using Domain.FluentApiClasses;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace algoriza_internship_288.Repository.DAL
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions options):base(options){}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().CreateApplicationUserRelation();
            builder.Entity<Doctor>().CreateDoctorRelation();
            builder.Entity<Appointment>().CreateAppointMentRelation();
            builder.Entity<Time>().CreateHourRelation();
            builder.Entity<Booking>().CreateBookingRelation();
            builder.Entity<Coupon>().CreateCoupobRelation();
            builder.Entity<Time>().CreateAppointMentRelation();
            builder.Entity<Specialization>().CreateSpecialization();
            builder.CreateAllRoles();
            builder.CreateSpecialization();
            base.OnModelCreating(builder);
        }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Time> Hours { get; set; }
        public DbSet<Specialization> Specializations { get; set; }

       
    }
}
