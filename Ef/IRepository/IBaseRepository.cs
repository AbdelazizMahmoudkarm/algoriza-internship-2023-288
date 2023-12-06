using algoriza_internship_288.Domain.AccountModels;
using algoriza_internship_288.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Repository.Repository
{
    public interface IBaseRepository<T> where T : class
    {
       public Task<int> CountAsync(string userType,DateTime search);
        public Task<bool> LoginAsync(Login login);
        public Task<string> LoginUsingJwtAsync(Login login);
        public Task<ApplicationUser> GetUserAsync(string userType, string userName);
        public ApplicationUser GetUserByEmail(string email);
        public string ProcessImage(IFormFile photo);
    }
}
