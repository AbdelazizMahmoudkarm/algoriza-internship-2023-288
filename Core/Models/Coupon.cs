using algoriza_internship_288.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace algoriza_internship_288.Core.Models
{
    public class Coupon
    {
        public int  Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public double Value { get; set; }
        public DiscountType DiscountType { get; set; }
        [Required]
        public int NumberOfRequestCompleted { get; set; }
        public virtual List<Booking> Bookings { get; set; }
        public bool Avaliable { get; set; }
    }
}
