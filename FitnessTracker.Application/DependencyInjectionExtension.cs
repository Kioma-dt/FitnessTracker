using FitnessTracker.Application.PasswordHasher;
using Microsoft.Extensions.DependencyInjection;
namespace FitnessTracker.Application
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddApplication(this
            IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            return services;
        }
    }
}
