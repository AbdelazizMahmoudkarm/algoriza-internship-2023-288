using algoriza_internship_288.Domain.Models.Enums;

namespace Domain.DtoClasses.Coupon
{
    public  class EditCouponDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double Value { get; set; }
        public DiscountType DiscountType { get; set; }
        public int NumberOfRequestCompleted { get; set; }
        public bool Avaliable { get; set; }
    }
}
