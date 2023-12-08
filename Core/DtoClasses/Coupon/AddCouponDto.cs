using algoriza_internship_288.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.DtoClasses.Coupon
{
    public  class AddCouponDto
    {
        [Required]
        public string Code{ get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public int NumberOfRequestCompleted { get; set; }
        [Required]
        public DiscountType DiscountType { get; set; }
    }
}
