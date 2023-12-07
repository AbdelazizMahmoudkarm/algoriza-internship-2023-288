using algoriza_internship_288.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.FluentApiClasses
{
    public static class Relations
    {
        public static void CreateDoctorRelation(this EntityTypeBuilder<Doctor> builder)
        {
            builder.HasOne(x => x.User).
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
            builder.HasMany(x => x.Bookings)
                    .WithOne(x => x.Coupon)
                    .HasForeignKey(x => x.CouponId)
                    .OnDelete(DeleteBehavior.SetNull);
        }
        public static void CreateAppointMentRelation(this EntityTypeBuilder<Appointment> builder)
        {
            builder.HasOne<Doctor>()
                    .WithMany(x => x.Appointments)
                    .HasForeignKey(x => x.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne<Booking>()
            //        .WithOne(x => x.Appointment)
            //        .HasForeignKey<Booking>(x => x.AppointmentId).OnDelete(DeleteBehavior.Restrict);

        }
        public static void CreateAppointMentRelation(this EntityTypeBuilder<Time> builder)
        {
            builder.HasOne(x => x.Appointment)
                 .WithMany(x => x.Times)
                 .HasForeignKey(x => x.AppointId)
                 .OnDelete(DeleteBehavior.Restrict);
        }
        public static void CreateBookingRelation(this EntityTypeBuilder<Booking> builder)
        {
            builder.Property(x => x.CouponId).IsRequired(false);
            builder.HasOne(x => x.Doctor)
                   .WithMany(x => x.Requests)
                   .HasForeignKey(x => x.DoctorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Patient)
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
        => builder.HasIndex(x => x.Name).IsUnique(true);
    }
}
