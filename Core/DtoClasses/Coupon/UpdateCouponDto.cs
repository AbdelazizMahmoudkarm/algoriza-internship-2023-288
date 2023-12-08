using algoriza_internship_288.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.DtoClasses.Coupon
{
    public  class UpdateCouponDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public DiscountType DiscountType { get; set; }
        [Required]
        public int NumberOfRequestCompleted { get; set; }
        public bool Avaliable { get; set; }
    }
}
