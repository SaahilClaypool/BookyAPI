using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BookyApi.API.Auth;
using BookyApi.API.Db;
using BookyApi.API.Models;
using BookyApi.API.Services.UseCases;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace BookyApi.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var jwtSecret = configuration.GetValue<string>("JWT_SECRET");
            JwtMiddleware.Secret = jwtSecret ?? JwtMiddleware.Secret;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // TODO: figure out better url detection
            UI.Program.ConfigureUIServices(services, "https://localhost:5001", true);
            services.AddControllers();
            services.AddRazorPages();
            services.AddDbContext<BookyContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("BookyDatabase"));
                options.UseLoggerFactory(loggerFactory);
                options.EnableSensitiveDataLogging();
            });
            services.AddLogging(options => options.AddConsole());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<CurrentUserAccessor, CurrentUserAccessor>();
            services.AddScoped((IServiceProvider services) =>
            {
                var userAccessor = services.GetService<CurrentUserAccessor>()!;
                return userAccessor.CurrentUser() ?? throw new UnauthorizedAccessException();
            });
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<BookyContext>()
            .AddDefaultTokenProviders();

            services.AddUseCases();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookyApi", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookyApi v1"));

            app.UseRouting();

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
