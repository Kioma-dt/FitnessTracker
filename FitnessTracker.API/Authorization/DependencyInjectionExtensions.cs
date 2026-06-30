using Microsoft.AspNetCore.Authorization;

namespace FitnessTracker.API.Authorization
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddAuthorizationRequirmentHandlers(this
            IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, WorkoutOwnerHandler>();

            services.AddScoped<IWorkoutOwnerAuthorizationService, WorkoutOwnerAuthorizationService>();

            return services;
        }
    }
}

