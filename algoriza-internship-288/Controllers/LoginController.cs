using algoriza_internship_288.Domain.AccountModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;

namespace algoriza_internship_288.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost("LoginUsingJWT")]
        public async Task<IActionResult> LoginJWtAsync(Login login)
        {
            string result = await _unitOfWork.Doctor.LoginUsingJwtAsync(login); 
            return Ok(result);       
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(Login login)
        {
            bool result = await _unitOfWork.Doctor.LoginAsync(login);
            return Ok(result);
        }
    }
}
