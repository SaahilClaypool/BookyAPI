using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Blazored.LocalStorage;

using BookyApi.Shared.DTO;

using Microsoft.AspNetCore.Components.Authorization;

// https://chrissainty.com/securing-your-blazor-apps-authentication-with-clientside-blazor-using-webapi-aspnet-core-identity/
namespace UI.Helpers
{
    public interface IAuthService
    {
        Task<LoginResultDTO> Login(LoginDTO loginModel);
        Task<bool> Refresh();
        Task Logout();
    }
    public class AuthService : IAuthService
    {
        private readonly HttpClient HttpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
        {
            HttpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task SaveToken(string username, string token)
        {
            await _localStorage.SetItemAsync("authToken", token);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(username);
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        public async Task<LoginResultDTO> Login(LoginDTO loginModel)
        {
            var loginAsJson = JsonSerializer.Serialize(loginModel);
            var response = await HttpClient.PostAsync("api/LogIn", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
            var loginResult = JsonSerializer.Deserialize<LoginResultDTO>(
                await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;

            if (!response.IsSuccessStatusCode)
            {
                return loginResult;
            }

            await SaveToken(loginModel.Username, loginResult.Token);

            return loginResult;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            HttpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<bool> Refresh()
        {
            System.Console.WriteLine("Checking sign in state");
            try
            {
                var response = await HttpClient.GetFromJsonAsync<LoginResultDTO>("api/LogIn/Refresh");
                if (response?.Success is false)
                {
                    return false;
                }

                await SaveToken(response!.UserName, response.Token);

                return true;
            }
            catch(System.Exception e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine(e.StackTrace);
                return false;
            }
        }
    }

}
