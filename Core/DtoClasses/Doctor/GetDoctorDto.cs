using Domain.DtoClasses.Appointment;

namespace Domain.DtoClasses.Doctor
{
    public class GetDoctorDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string SpecializeNmae { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public double CheckPrice { get; set; }
        public double ReCheckPrice { get; set; }
        public List<GetAppointmentDto> Appointments { get; set; }
    }
}
