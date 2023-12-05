using Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace algoriza_internship_288.Core.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [Required]
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
        [Required]
        public string PatientId { get; set; }
        public virtual ApplicationUser Patient { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }
        [Required]
        public int HourId { get; set; }
        public virtual Time Hour { get; set; }
        [Required]
        public RequestType Status { get; set; }
        public double Price { get; set; }
        [AllowNull]
        public int? CouponId { get; set; }
        public virtual Coupon Coupon { get; set; }
    }
}
