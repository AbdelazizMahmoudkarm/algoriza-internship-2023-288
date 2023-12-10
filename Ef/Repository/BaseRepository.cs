using algoriza_internship_288.Domain.AccountModels;
using algoriza_internship_288.Domain.Models;
using algoriza_internship_288.Domain.Models.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Repository.Repository
{
    public class BaseRepository : IBaseRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfigurationRoot _config;
        public BaseRepository(UserManager<ApplicationUser> usermanager,
                SignInManager<ApplicationUser> signInManager)
        {
            _userManager = usermanager;
            _signInManager = signInManager;
              _signInManager= signInManager;
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }
        public async Task<int> CountAsync(string userType, DateTime search)
        {
            if (!search.Equals(DateTime.MinValue))
                return (await _userManager.GetUsersInRoleAsync(userType)).Count(x => x.DateOfAdd >= search);
            else
                return (await _userManager.GetUsersInRoleAsync(userType)).Count;
        }

        public async Task<bool> LoginAsync(Login login)
        => (await _signInManager.PasswordSignInAsync(login.UserName, login.Password, false, false)).Succeeded;
        
       public async Task<string> LoginUsingJwtAsync(Login login)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(login.UserName);
            if (user is not null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
                if (result.Succeeded)
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                    JwtSecurityToken token = new JwtSecurityToken(_config["Jwt:Issuer"],
                        _config["Jwt:Audience"], null, expires: DateTime.Now.AddMinutes(15), signingCredentials: credentials);
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
            }
            return null;
        }
        #region ExternalLogin
        //1
        public async Task<List<AuthenticationScheme>> ExternalLoginAsync()
            => (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        
        //2
        public  AuthenticationProperties AuthenticationProperties(string provider,string returnUrl)
             =>(_signInManager.ConfigureExternalAuthenticationProperties(provider,returnUrl));
        //3
        public async Task<bool> CreateUserWithExternalLoginCallBackAsync()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return false;
            var result = await _signInManager
                .ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
                return true;
            else
            {
                string email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email is not null)
                {
                    ApplicationUser user = await _userManager.FindByEmailAsync(email);
                    if (user != null)
                    {
                        await _userManager.AddLoginAsync(user, info);
                        await _signInManager.SignInAsync(user, false);
                        return true;
                    }
                    else
                    {
                        user = new ApplicationUser()
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Name).GetUserName(),
                            Email = email,
                        };
                        IdentityResult createuserResult = await _userManager.CreateAsync(user);
                        if (createuserResult.Succeeded)
                        {
                            createuserResult = await _userManager.AddToRoleAsync(user, UserType.Patient.ToString());
                            IdentityResult createuserLogins = await _userManager.AddLoginAsync(user, info);
                            if (createuserLogins.Succeeded)
                            {
                                await _signInManager.SignInAsync(user, false);
                                return true;
                            }
                        }
                        return createuserResult.Succeeded;
                    }
                }
            }
            return false;
        }

        #endregion
        public async Task<ApplicationUser> GetUserAsync(string userType, string userName)
            => (await _userManager.GetUsersInRoleAsync(userType))
                .FirstOrDefault(x => x.UserName.Equals(userName));

        public ApplicationUser GetUserByEmail(string email)
             => _userManager.Users.FirstOrDefault(x => x.Email.Equals(email));
    }
}
