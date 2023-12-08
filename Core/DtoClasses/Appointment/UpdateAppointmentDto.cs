using algoriza_internship_288.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.DtoClasses.Appointment
{
    public class UpdateAppointmentDto
    {
        //public int dayId { get; set; }
        //public Days Day { get; set; }
        [Required]
        public int HourId { get; set; }
        [Required]
        public double Hour { get; set; }

    }
}
