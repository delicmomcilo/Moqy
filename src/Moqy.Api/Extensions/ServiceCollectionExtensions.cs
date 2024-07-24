using Moqy.Api.Services;
using Moqy.Api.Configuration;

namespace Moqy.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LlmServiceOptions>(configuration.GetSection("LlmService"));
            services.AddSingleton<ILlmService, LlmService>();
            services.AddSingleton<IMockDataService, MockDataService>();

            return services;
        }
    }
}