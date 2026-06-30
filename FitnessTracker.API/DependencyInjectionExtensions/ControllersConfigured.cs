using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.API.DependencyInjectionExtensions
{
    public static class ControllersConfigured
    {
        public static IServiceCollection AddControllersConfigured(this IServiceCollection services)
        {
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .SelectMany(x => x.Value!.Errors.Select(x => x.ErrorMessage))
                        .ToList();

                        return new BadRequestObjectResult(new ProblemDetails
                        {
                            Status = 400,
                            Title = "Validation Failed",
                            Detail = "Errors while validating request",
                            Extensions = { ["errors"] = errors }
                        });
                    };
                });

            return services;
        }
    }
}
