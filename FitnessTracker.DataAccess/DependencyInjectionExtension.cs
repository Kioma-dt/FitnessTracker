using FitnessTracker.Application.Interfaces.Repositories;
using FitnessTracker.Application.PasswordHasher;
using FitnessTracker.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

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
