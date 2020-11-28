using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            ConfigureUIServices(builder.Services, builder.HostEnvironment.BaseAddress);

            await builder.Build().RunAsync();
        }

        public static void ConfigureUIServices(IServiceCollection services, string baseUri, bool server = false)
        {
            // TODO: make this better
            services.AddScoped(sp =>
            {
                if (server)
                {
                    var httpClientHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                    };
                    return new HttpClient(handler: httpClientHandler)
                    {
                        BaseAddress = new Uri("https://localhost:5001"),
                    };
                }
                else
                {
                    return new HttpClient { BaseAddress = new(baseUri) };
                }
            });

            services.AddScoped(sp => new HostEnvironmentService { IsServer = server });
        }

        class HostEnvironmentService
        {
            public bool IsServer { get; set; } = false;
        }
    }
}
