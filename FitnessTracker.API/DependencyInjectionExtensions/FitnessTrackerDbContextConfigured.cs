using FitnessTracker.DataAccess;
using FitnessTracker.Shared.Exceptions.InternalServerError;

using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.API.DependencyInjectionExtensions
{
    public static class FitnessTrackerDbContextConfigured
    {
        public static IServiceCollection AddFitnessTrackerDbContextConfigured(this IServiceCollection services)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var db = Environment.GetEnvironmentVariable("DB_NAME");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

            if (host is null ||
                port is null ||
                db is null ||
                user is null ||
                password is null)
            {
                throw new EnviormnetVariableNotFoundException("Can't Find Enviorment Variables for Connection String");
            }

            var connectionString =
                    $"Host={host};Port={port};Database={db};Username={user};Password={password}";

            services.AddDbContext<FitnessTrackerDbContext>(options =>
                options.UseNpgsql(connectionString,
                o =>
                {
                    o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    o.EnableRetryOnFailure(3, TimeSpan.FromSeconds(30), null);
                }));

            return services;
        }
    }
}
