using FitnessTracker.Shared.Exceptions.InternalServerError;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace FitnessTracker.API.DependencyInjectionExtensions
{
    public static class AuthenticationConfigured
    {
        public static IServiceCollection AddAuthenticationConfigured(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var authOptions = configuration.GetSection("Authentication");
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");

            if (authOptions is null
                || authOptions["Issuer"] is null
                || authOptions["Audience"] is null)
            {
                throw new ConfigurationSectionNotFoundException("No configuration params for authentication");
            }

            if (jwtKey is null)
            {
                throw new EnviormnetVariableNotFoundException("No enviormnet variables for authorization");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidIssuer = authOptions["Issuer"],
                       ValidateAudience = true,
                       ValidAudience = authOptions["Audience"],
                       ValidateLifetime = true,
                       IssuerSigningKey = key,
                       ValidateIssuerSigningKey = true
                   };
                   options.Events = new JwtBearerEvents
                   {
                       OnTokenValidated = context =>
                       {
                           var userId = context.Principal?
                               .FindFirst(ClaimTypes.NameIdentifier)
                               ?.Value;

                           if (string.IsNullOrEmpty(userId))
                           {
                               context.Fail("JWT token does not contain user id");
                           }

                           return Task.CompletedTask;
                       },
                       OnChallenge = context =>
                       {
                           context.HandleResponse();

                           context.Response.StatusCode = 401;
                           context.Response.ContentType = "application/json";

                           return context.Response.WriteAsJsonAsync(new ProblemDetails
                           {
                               Status = 401,
                               Title = "Unauthorized",
                               Detail = "Token is missing or invalid"
                           });
                       },

                       OnForbidden = context =>
                       {
                           context.Response.StatusCode = 403;
                           context.Response.ContentType = "application/json";

                           return context.Response.WriteAsJsonAsync(new ProblemDetails
                           {
                               Status = 403,
                               Title = "Forbiden",
                               Detail = "You don't have access to this resource"
                           });
                       }
                   };

               });
            return services;
        }
    }
}
