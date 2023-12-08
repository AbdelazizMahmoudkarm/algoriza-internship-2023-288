using algoriza_internship_288.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.DtoClasses.Booking
{
    public  class AddBookingDto
    {
        [Required]
        public int DoctorId { get; set; }
        [Required]
        public int TimeId { get; set; }
        public bool IsCheck {  get; set; }
        
        public string CouponCode {  get; set; }
    }
}
