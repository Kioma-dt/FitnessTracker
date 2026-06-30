using FitnessTracker.Application.Interfaces.Authentication;
using FitnessTracker.Application.Interfaces.DataSellection.Filtering;
using FitnessTracker.Application.Interfaces.DataSellection.Ordering;
using FitnessTracker.Application.Interfaces.Images;
using FitnessTracker.Inrastructure.Authentication.JwtTokenFactory;
using FitnessTracker.Inrastructure.Authentication.PasswordHasher;
using FitnessTracker.Inrastructure.DataSellection.Filterring.WorkoutFilters;
using FitnessTracker.Inrastructure.DataSellection.Ordering.WorkoutOrders;
using FitnessTracker.Inrastructure.Images.PhotosRemoteStorage;
using FitnessTracker.Inrastructure.Images.StreamImageChecker;

using Imagekit;
using Microsoft.Extensions.DependencyInjection;
namespace FitnessTracker.Application
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddInfrastructure(this
            IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

            services.AddScoped<IJwtTokenFactory, IdentityJwtTokenFactory>();

            services.AddSingleton<ImageKitClient>();
            services.AddScoped<IImageKitClientWrapper, ImageKitClientWrapper>();
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
    }
}
