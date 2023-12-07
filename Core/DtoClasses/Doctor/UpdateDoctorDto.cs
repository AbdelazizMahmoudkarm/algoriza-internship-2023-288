using algoriza_internship_288.Domain.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.DtoClasses.Doctor
{
    public  class UpdateDoctorDto 
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int SpecializeId { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double CheckPrice { get; set; }
        public double ReCheckPrice { get; set; }
    }
}
