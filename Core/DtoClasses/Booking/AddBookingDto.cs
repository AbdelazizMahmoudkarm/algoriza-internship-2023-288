using algoriza_internship_288.Domain.Models.Enums;

namespace Domain.DtoClasses.Booking
{
    public  class AddBookingDto
    {
        public int DoctorId { get; set; }
        public int TimeId { get; set; }
      //  public int AppointmentId { get; set; }
        //  public double  Time { get; set; }
        public bool IsCheck {  get; set; }
        //public string  DoctorName { get; set; }
        public string CouponCode {  get; set; }
       // public Days Day { get; set; }
    }
}
