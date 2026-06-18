using FitnessTracker.Application.JwtTokenFactory;
using FitnessTracker.Application.PasswordHasher;
using FitnessTracker.Application.PhotosRemoteStorage;
using FitnessTracker.Application.StreamImageChecker;
using FitnessTracker.Application.WorkoutFilters;
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
            return services;
        }
    }
}
