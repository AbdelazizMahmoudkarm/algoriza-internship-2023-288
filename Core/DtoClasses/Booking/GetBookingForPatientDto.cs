namespace Domain.DtoClasses.Booking
{
    public  class GetBookingForPatientDto
    {
        public string  DoctorName { get; set; }
        public string DoctorImage { get; set; }
        public string SpecializeName { get; set; }
        public string Day { get; set; }
        public double Time { get; set; }
        public double Price { get; set; }
        public string DiscountCode { get; set; }
        public double FinalPrice { get; set; }
        public string Status { get; set; }
        public DateTime? BookingDate { get; set; }

    }
}
