using Microsoft.OpenApi;

namespace FitnessTracker.API.DependencyInjectionExtensions
{
    public static class SwaggerGenConfigured
    {
        public static IServiceCollection AddSwaggerGenConfugured(this
            IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Fitness Tracker", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Input JWT token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });
            return services;
        }
    }
}
