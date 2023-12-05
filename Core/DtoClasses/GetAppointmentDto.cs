using algoriza_internship_288.Core.Models.Enums;

namespace Domain.DtoClasses
{
    public class GetAppointmentDto
    {
        public string Days { get; set; }
        public List<TimeSpan> Times { get; set; }
    }
}
