using Microsoft.Extensions.DependencyInjection;

namespace BookyApi.API.Services.UseCases
{
    public static class UseCaseExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services) =>
            services.AddScoped<PopulateFromClipboard>();
    }
}