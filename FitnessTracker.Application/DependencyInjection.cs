using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.Application
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddApplication(this
            IServiceCollection services)
        {
            services.AddMediatR(conf =>

                conf.RegisterServicesFromAssembly(typeof(DependencyInjectionExtension)
                    .Assembly));
            return services;
        }
    }
}
