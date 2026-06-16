using Microsoft.EntityFrameworkCore.Design;

namespace FitnessTracker.DataAccess
{
    public class DesignTimeFitnessTrackerDbContextFactory
        : IDesignTimeDbContextFactory<FitnessTrackerDbContext>
    {
        public FitnessTrackerDbContext CreateDbContext(string[] args)
        {
            DotNetEnv.Env.TraversePath().Load("designtime.env");

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
                throw new EnviormnetVariableNotFound("Can't Find Enviorment Variables for Connection String");
            }

            var connectionString =
                    $"Host={host};Port={port};Database={db};Username={user};Password={password}";

            var optionsBuilder = new DbContextOptionsBuilder<FitnessTrackerDbContext>()
                .UseNpgsql(connectionString);

            return new FitnessTrackerDbContext(optionsBuilder.Options);
        }
    }
}
