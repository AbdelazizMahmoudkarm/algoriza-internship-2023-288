namespace algoriza_internship_288.Core.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int SpecializeId { get; set; }
        public virtual Specialization Specialization { get; set; }
        public virtual List<Appointment> Appointments { get; set; }
        public virtual List<Booking> Requests { get; set; }
        public double CheckPrice { get; set; }
        public double ReCheckPrice { get; set; }
    }
}
