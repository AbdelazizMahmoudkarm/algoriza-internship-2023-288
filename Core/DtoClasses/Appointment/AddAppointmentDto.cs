using algoriza_internship_288.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.DtoClasses.Appointment
{
    public class AddAppointmentDto
    {
        [Required]
        public Days Days { get; set; }
        [Required]
        public List<double> Times { get; set; }
    }
}
