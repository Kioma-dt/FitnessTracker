using FitnessTracker.Shared.Exceptions.InternalServerError;

namespace FitnessTracker.API.DependencyInjectionExtensions;

public static class RedisConfigured
{
    public static IServiceCollection AddRedisConfigured(this IServiceCollection services)
    {
        var host = Environment.GetEnvironmentVariable("REDIS_HOST");
        var port = Environment.GetEnvironmentVariable("REDIS_PORT");
        if (host is null ||
            port is null)
        {
            throw new EnviormnetVariableNotFoundException("Can't Find Enviorment Variables for Redis");
        }

        var connectionString = $"{host}:{port}";

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.InstanceName = "fitness:";
        });
        return services;
    }
}