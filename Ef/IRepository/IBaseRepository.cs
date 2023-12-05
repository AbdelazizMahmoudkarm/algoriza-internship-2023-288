using algoriza_internship_288.Core.AccountModels;
using algoriza_internship_288.Core.Models;

namespace Repository.Repository
{
    public interface IBaseRepository<T> where T : class
    {
       public Task<int> CountAsync(string userType,DateTime search);
       public Task<bool> LoginAsync(Login login);
        public Task<ApplicationUser> GetUserAsync(string userType, string userName);
        public ApplicationUser GetUserByEmail(string email);
    }
}
