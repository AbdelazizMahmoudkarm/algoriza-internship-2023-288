using algoriza_internship_288.Core.Models.Enums;

namespace Domain.DtoClasses.Coupon
{
    public  class EditCouponDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double Value { get; set; }
        public DateTime ExppireDate { get; set; }
        public DiscountType DiscountType { get; set; }
        public int NumberOfRequestCompleted { get; set; }
        public bool Avaliable { get; set; }
    }
}
