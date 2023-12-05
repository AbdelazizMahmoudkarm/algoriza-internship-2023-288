using algoriza_internship_288.Core.Models.Enums;

namespace Domain.DtoClasses.Appointment
{
    public class AddAppointmentDto
    {
        public Days Days { get; set; }
        public List<double> Times { get; set; }
    }
}
