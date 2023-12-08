using algoriza_internship_288.Domain.Models;
using Domain.DtoClasses.Appointment;
using System.ComponentModel.DataAnnotations;

namespace Domain.DtoClasses.Doctor
{
    public class AddDoctorAppointmentsDto
    {
        [Required]
        public double CheckPrice { get; set; }
        [Required]
        public double ReCheckPrice { get; set; }
        
        public List<AddAppointmentDto> Appointments { get; set; }

    }
}
