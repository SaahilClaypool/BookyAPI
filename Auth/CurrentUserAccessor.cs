using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookyApi.Db;
using BookyApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BookyApi.Auth
{
    public class CurrentUserAccessor
    {
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly BookyContext Context;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor, BookyContext context)
        {
            HttpContextAccessor = httpContextAccessor;
            Context = context;
        }

        public async Task<User> FindByUsername(string username)
        {
            return await Context.Users.Where(user => user.UserName == username).FirstAsync();
        }

        public async Task<User> FindById(string id)
        {
            return await Context.Users.Where(user => user.Id == id).FirstAsync();
        }

        public User? CurrentUser() => (User?)HttpContextAccessor.HttpContext?.Items["User"];
    }
}
