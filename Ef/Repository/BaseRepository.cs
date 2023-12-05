using algoriza_internship_288.Core.AccountModels;
using algoriza_internship_288.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Repository.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public BaseRepository(UserManager<ApplicationUser> usermanager,
                SignInManager<ApplicationUser> signInManager)
        {
            _userManager = usermanager;
            _signInManager = signInManager;
              _signInManager= signInManager;
        }

        public async Task<int> CountAsync(string userType, DateTime search)
        {
            
            if (!search.Equals(DateTime.MinValue))
                return (await _userManager.GetUsersInRoleAsync(userType)).Count(x => x.DateOfAdd >= search);
            else
                return (await _userManager.GetUsersInRoleAsync(userType)).Count;


        }

        public async Task<bool> LoginAsync(Login login)
        {
            return (await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false)).Succeeded;
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
    }
}
