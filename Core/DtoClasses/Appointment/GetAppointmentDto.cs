using algoriza_internship_288.Domain.Models.Enums;

namespace Domain.DtoClasses.Appointment
{
    public class GetAppointmentDto
    {
        public string Days { get; set; }
        public List<TimeSpan> Times { get; set; }
    }
}
