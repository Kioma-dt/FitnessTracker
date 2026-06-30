using FitnessTracker.API.Authorization;

namespace FitnessTracker.API.DependencyInjectionExtensions
{
    public static class AuthorizationConfigured
    {
        public static IServiceCollection AddAuthorizationConfigured(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "WorkoutOwner",
                    policy =>
                    {
                        policy.Requirements.Add(
                            new WorkoutOwnerRequirement());
                    });
            });
            return services;
        }
    }
}
