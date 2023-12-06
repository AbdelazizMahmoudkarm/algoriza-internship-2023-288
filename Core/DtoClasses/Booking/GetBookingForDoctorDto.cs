using algoriza_internship_288.Domain.Models.Enums;

namespace Domain.DtoClasses.Booking
{
    public  class GetBookingForDoctorDto
    {
        public string Patientname { get; set; }
        public string Image { get; set; }
        public int age  { get; set; }
        public string Phone { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public TimeSpan Hour { get; set; }
        public string Status { get; set; }
        public DateTime BookingDate { get; set; }
        public string Day { get; set; }

    }
}
