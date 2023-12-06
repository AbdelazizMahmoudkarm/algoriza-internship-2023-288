using algoriza_internship_288.Domain.AccountModels;
using algoriza_internship_288.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Repository.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfigurationRoot _config;
        //private readonly IWebHostEnvironment _hostEnvironment;
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
                        _config["Jwt:Audence"], null, expires: DateTime.Now.AddMinutes(15), signingCredentials: credentials);
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
            }
            return null;
        }
        public async Task<ApplicationUser> GetUserAsync(string userType, string userName)
        {
            return (await _userManager.GetUsersInRoleAsync(userType))
                .FirstOrDefault(x => x.UserName.Equals(userName));
        }
        public ApplicationUser GetUserByEmail( string email)
        {
            return _userManager.Users.FirstOrDefault(x => x.Email.Equals(email));
        }
        public  string ProcessImage(IFormFile photo)
        {
            string uniqueName = default;
            if (photo is not null)
            {
                string path = Path.Combine("~\\..\\wwwroot", "Images");
                uniqueName = Guid.NewGuid() + "_" + photo.FileName;
                string Fullpath = Path.Combine(path, uniqueName);
                using FileStream fileStream = new(Fullpath, FileMode.Create);
                photo.CopyTo(fileStream);
            }
            return uniqueName;
        }
    }
}
