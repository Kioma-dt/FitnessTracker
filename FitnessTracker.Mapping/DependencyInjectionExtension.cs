using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
namespace FitnessTracker.Mapping
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddMappers(this
            IServiceCollection services)
        {
            services.AddSingleton<TypeAdapterConfig>(GetMappingConfig());
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }

        private static TypeAdapterConfig GetMappingConfig()
        {
            var config = new TypeAdapterConfig();
            new RegisterMapper().Register(config);

            config.Compile();

            return config;
        }
    }
}
