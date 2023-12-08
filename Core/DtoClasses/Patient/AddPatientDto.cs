using Microsoft.AspNetCore.Http;

namespace Domain.DtoClasses.Patient
{
    public class AddPatientDto : BaseDto
    {
        public IFormFile Image { get; set; }
    }
}
