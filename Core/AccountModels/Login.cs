using System.ComponentModel.DataAnnotations;

namespace algoriza_internship_288.Domain.AccountModels
{
    public class Login
    {
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
