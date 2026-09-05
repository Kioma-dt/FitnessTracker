using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Application.Interfaces.Repositories;
using FitnessTracker.DataAccess.Repositories.Cached;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.DataAccess
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddRepositories(this
            IServiceCollection services)
        {
            services.AddScoped<UsersRepository>();
            services.AddScoped<IUsersRepository, CachedUsersRepository>();
            services.AddScoped<WorkoutsRepository>();
            services.AddScoped<IWorkoutsRepository, CachedWorkoutsRepository>();
            return services;
        }
    }
}
