using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.DtoClasses.Doctor
{
    public class AddDoctorDto : BaseDto
    {
        [Required]
        public IFormFile Image { get; set; }
        [Required]
        public int  SpecializeId { get; set; }
    }
}
