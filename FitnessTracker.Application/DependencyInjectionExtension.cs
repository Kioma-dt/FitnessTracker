using FitnessTracker.Application.JwtTokenFactory;
using FitnessTracker.Application.Mappers;
using FitnessTracker.Application.PasswordHasher;
using FitnessTracker.Application.PhotosRemoteStorage;
using FitnessTracker.Application.StreamImageChecker;
using FitnessTracker.Application.WorkoutFilters;
using FitnessTracker.Application.WorkoutOrdering;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
namespace FitnessTracker.Application
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddApplication(this
            IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

            services.AddScoped<IJwtTokenFactory, IdentityJwtTokenFactory>();

            services.AddScoped<IPhotosRemoteStorage, ImageKitRemoteStorage>();

            services.AddScoped<IStreamImageChecker, SkiaSharpStreamImageChecker>();

            services.AddScoped<IWorkoutFilter, FromDateWorkoutFilter>();
            services.AddScoped<IWorkoutFilter, ToDateWorkoutFilter>();
            services.AddScoped<IWorkoutFilter, MinDurationWorkoutFilter>();
            services.AddScoped<IWorkoutFilter, MaxDurationWorkoutFilter>();
            services.AddScoped<IWorkoutFilter, TypeWorkoutFilter>();

            services.AddScoped<IWorkoutFilterExpressionBuilder, WorkoutFilterExpressionBuilder>();

            services.AddScoped<IWorkoutOrder, WorkoutOrderByDate>();
            services.AddScoped<IWorkoutOrder, WorkoutOrderByBurnedCalories>();

            services.AddScoped<IWorkoutOrderingApplier, WorkoutOrderingApplier>();
            return services;
        }

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
