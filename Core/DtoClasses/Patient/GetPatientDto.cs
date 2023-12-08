using algoriza_internship_288.Domain.Models.Enums;

namespace Domain.DtoClasses.Patient
{
    public class GetPatientDto
    {
        public string PatientName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public double Age { get; set; }
    }
}
