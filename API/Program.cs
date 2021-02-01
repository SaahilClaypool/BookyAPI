using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookyApi.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) 
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var hostUrls = Environment.GetEnvironmentVariable("SERVER_URLS");
                    System.Console.WriteLine(hostUrls);
                    if (hostUrls is not null)
                    {
                        webBuilder.UseUrls(hostUrls.Split(";").ToArray());
                    }
                    webBuilder.UseStartup<Startup>();
                });

            return host;
        }
    }
}
