using FitnessTracker.Shared.Options.Redis;

namespace FitnessTracker.API.DependencyInjectionExtensions;

public static class OptionsConfigured
{
    public static IServiceCollection ConfigureMyOptions(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.Configure<RedisTTLOptions>(configuration.GetSection("RedisTTL"));
        return services;
    }
}