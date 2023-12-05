namespace Domain.DtoClasses.Booking
{
    public  class GetBookingForPatientDto
    {
        public string  DoctorName { get; set; }
        public string Image { get; set; }
        public string SpecializeName { get; set; }
        public string Day { get; set; }
        public double Time { get; set; }
        public double Price { get; set; }
        public int CouponId { get; set; }
        public double FinalPrice { get; set; }
        public string Status { get; set; }
        public DateTime? BookingDate { get; set; }

    }
}
