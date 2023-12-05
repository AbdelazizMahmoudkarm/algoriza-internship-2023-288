using algoriza_internship_288.Core.AccountModels;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;

namespace algoriza_internship_288.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(Login login)
        {
            return Ok(await _unitOfWork.Doctor.LoginAsync(login));
        }
    }
}
