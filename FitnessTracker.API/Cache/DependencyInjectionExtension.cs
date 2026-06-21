using FitnessTracker.API.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace FitnessTracker.API.Cache
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddETagCache(this
            IServiceCollection services)
        {
            services.AddScoped<IETagGenerator, ETagGenerator>();

            return services;
        }
    }
}
