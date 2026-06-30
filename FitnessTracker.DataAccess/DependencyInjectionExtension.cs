using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Application.Interfaces.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.DataAccess
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddRepositories(this
            IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IWorkoutsRepository, WorkoutsRepository>();
            return services;
        }
    }
}
