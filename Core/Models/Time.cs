namespace algoriza_internship_288.Domain.Models
{
    public class Time
    {
        public int Id { get; set; }
        public int AppointId { get; set; }
        public virtual Appointment Appointment { get; set; }
        public TimeSpan ExistHour { get; set; }
    }
}
