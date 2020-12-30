using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using BookyApi.API.Db;
using BookyApi.API.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookyApi.API.Auth
{
    public class CurrentUserAccessor
    {
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly BookyContext Context;

        public UserManager<User> Manager { get; }

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor, BookyContext context, UserManager<User> manager)
        {
            HttpContextAccessor = httpContextAccessor;
            Context = context;
            Manager = manager;
        }

        public async Task<User> FindByUsername(string username)
        {
            return await Context.Users.Where(user => user.UserName == username).FirstAsync();
        }

        public async Task<User> FindById(string id)
        {
            return await Context.Users.Where(user => user.Id == id).FirstAsync();
        }

        public Task<User> CurrentUser() => FindById(Manager.GetUserId(
            HttpContextAccessor?.HttpContext?.User));
        public string CurrentUserId() => Manager.GetUserId(
            HttpContextAccessor?.HttpContext?.User);
    }
}
