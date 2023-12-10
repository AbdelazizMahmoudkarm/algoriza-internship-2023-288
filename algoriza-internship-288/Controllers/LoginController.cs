using algoriza_internship_288.Domain.AccountModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
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

        [HttpGet("ExternalLogin")]
        public async Task<IActionResult> ExternalLogin()
        {
           var ex= await _unitOfWork.ExternalLoginAsync();
            //  var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback");
            var properties = _unitOfWork.AuthenticationProperties(ex.FirstOrDefault().Name, "./api/Login/Callback");//redirectUrl);
            return new ChallengeResult(ex.FirstOrDefault().Name,properties);
        }
        [HttpGet("Callback")]
        
        public async Task<IActionResult> Callback()
        {
            bool result = await _unitOfWork.CreateUserWithExternalLoginCallBackAsync();
            return Ok(result);
        }

        [HttpPost("LoginUsingJWT")]
        public async Task<IActionResult> LoginJWtAsync(Login login,bool arabic=false)
        {
            Localization.Arabic = arabic;
            string result = await _unitOfWork.LoginUsingJwtAsync(login); 
            return Ok(result);       
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(Login login,bool arabic)
        {
            Localization.Arabic = arabic;
            bool result = await _unitOfWork.LoginAsync(login);
            return Ok(result);
        }
    }
}
