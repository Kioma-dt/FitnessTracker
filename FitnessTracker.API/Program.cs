using FitnessTracker.API.Authorization;
using FitnessTracker.API.ExceptionHandler;
using FitnessTracker.Application;
using FitnessTracker.DataAccess;
using FitnessTracker.API.DependencyInjectionExtensions;


namespace FitnessTracker.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var envState = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = WebApplication.CreateBuilder(args);

            if (envState is not null && envState == "Development")
            {
                DotNetEnv.Env.Load("enviorment.env");
            }
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddSwaggerGenConfugured();

            builder.Services.AddAuthorizationConfigured();

            builder.Services.AddAuthenticationConfigured(builder.Configuration);

            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            builder.Services.AddFitnessTrackerDbContextConfigured();

            builder.Services.AddControllersConfigured();

            builder.Services
                .AddApplication()
                .AddInfrastructure()
                .AddMappers()
                .AddRepositories()
                .AddAuthorizationRequirmentHandlers();

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler();

            app.MapControllers();

            app.Run();
        }
    }
}
