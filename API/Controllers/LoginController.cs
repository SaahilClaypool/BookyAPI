using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookyApi.API.Auth;
using BookyApi.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using BookyApi.Shared.DTO;
using System.Text.Json;
using System.IO;

// https://devblogs.microsoft.com/aspnet/asp-net-core-authentication-with-identityserver4/
// has example of the message type needed to get token from identityserver4
namespace BookyApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger Logger;
        private CurrentUserAccessor CurrentUserAccessor { get; set; }

        public SignInManager<User> SignInManager { get; }
        public UserManager<User> UserManager { get; }

        public LoginController(
            SignInManager<User> signInManager,
            ILogger<LoginController> logger,
            CurrentUserAccessor currentUserAccessor,
            UserManager<User> userManager
            )
        {
            SignInManager = signInManager;
            Logger = logger;
            CurrentUserAccessor = currentUserAccessor;
            UserManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<bool> Register(
            [FromBody] LoginDTO details
        )
        {
            Logger.LogCritical($"Register: {JsonSerializer.Serialize(details)}");
            Logger.LogCritical($"Register: {JsonSerializer.Serialize(details)}");
            var user = new User()
            {
                UserName = details.Username,
                Email = details.Username
            };
            var result = await UserManager.CreateAsync(user, details.Password);
            return result.Succeeded;
        }

        [HttpPost]
        public async Task<LoginResultDTO> LogIn(
            [FromBody] LoginDTO details
        )
        {
            Logger.LogCritical($"user: {JsonSerializer.Serialize(details)}");
            var result = await SignInManager.PasswordSignInAsync(details.Username, details.Password, true, false);
            if (!result.Succeeded)
            {
                return new LoginResultDTO { Success = false, Token = result.ToString() };
            }
            var user = await CurrentUserAccessor.FindByUsername(details.Username);
            var token = JwtMiddleware.GenerateJwtToken(user, Logger);
            Logger.LogDebug($"Logged in {details.Username} with token {token}");

            return new LoginResultDTO { Success = true, Token = token };
        }

        [HttpPost("Logout")]
        public Task LogOut()
        {
            SignOut();
            return SignInManager.SignOutAsync();
        }

    }
}
