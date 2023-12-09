using algoriza_internship_288.Domain.AccountModels;
using algoriza_internship_288.Domain.Models;
using Microsoft.AspNetCore.Authentication;

namespace Repository.Repository
{
    public interface IBaseRepository
    {
       public Task<int> CountAsync(string userType,DateTime search);
        public Task<bool> LoginAsync(Login login);
        public Task<string> LoginUsingJwtAsync(Login login);
       
        public Task<ApplicationUser> GetUserAsync(string userType, string userName);
        public ApplicationUser GetUserByEmail(string email);



        public Task<List<AuthenticationScheme>> ExternalLoginAsync();
        public AuthenticationProperties AuthenticationProperties(string provider, string returnUrl);
        public Task<bool> CreateUserWithExternalLoginCallBackAsync();
    }
}
