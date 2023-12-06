using algoriza_internship_288.Domain.Models;
using Domain.DtoClasses.Appointment;

namespace Domain.DtoClasses.Doctor
{
    public class AddDoctorAppointmentsDto
    {
        public double CheckPrice { get; set; }
        public double ReCheckPrice { get; set; }
        //public int DoctorId { get; set; }
        public List<AddAppointmentDto> Appointments { get; set; }

    }
}
