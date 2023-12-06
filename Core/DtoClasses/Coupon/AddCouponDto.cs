using algoriza_internship_288.Domain.Models.Enums;

namespace Domain.DtoClasses.Coupon
{
    public  class AddCouponDto
    {
        public string Code{ get; set; }
        public double Value { get; set; }
        public int NumberOfRequestCompleted { get; set; }
        public DiscountType DiscountType { get; set; }
    }
}
