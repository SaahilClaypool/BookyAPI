using System;
using System.Net.Http;
using System.Threading.Tasks;

using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using UI.Helpers;

namespace UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            ConfigureUIServices(builder.Services, builder.HostEnvironment.BaseAddress);
            builder.Services.AddLogging();

            await builder.Build().RunAsync();
        }

        public static void ConfigureUIServices(IServiceCollection services, string baseUri, bool server = false)
        {
            services.AddScoped(sp =>
                new HttpClient { BaseAddress = new(baseUri) }
            );

            services.AddScoped(sp => new HostEnvironmentService { IsServer = server });
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();
            services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            services.AddScoped<IAuthService, AuthService>();
        }

    }
    public class HostEnvironmentService
    {
        public bool IsServer { get; set; } = false;
    }
}
