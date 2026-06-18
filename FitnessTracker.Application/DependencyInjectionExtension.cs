using FitnessTracker.Application.JwtTokenFactory;
using FitnessTracker.Application.PasswordHasher;
using FitnessTracker.Application.PhotosRemoteStorage;
using FitnessTracker.Application.StreamImageChecker;
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
            return services;
        }
    }
}
