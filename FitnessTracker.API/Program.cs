using FitnessTracker.API.ExceptionHandler;
using FitnessTracker.Application;
using FitnessTracker.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

namespace FitnessTracker.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            DotNetEnv.Env.Load("enviorment.env");

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddSwaggerGen(options =>
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

            builder.Services.AddAuthorization();

            var authOptions = builder.Configuration.GetSection("Authentication");
            var jwtKey = builder.Configuration.GetValue<string>("JWT_KEY");

            if (authOptions is null || jwtKey is null)
            {
                throw new EnviormnetVariableNotFoundException("No enviormnet variables for authorization");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            var host = builder.Configuration.GetValue<string>("DB_HOST");
            var port = builder.Configuration.GetValue<string>("DB_PORT");
            var db = builder.Configuration.GetValue<string>("DB_NAME");
            var user = builder.Configuration.GetValue<string>("DB_USER");
            var password = builder.Configuration.GetValue<string>("DB_PASSWORD");

            if (host is null ||
                port is null ||
                db is null ||
                user is null ||
                password is null)
            {
                throw new EnviormnetVariableNotFoundException("Can't Find Enviorment Variables for Connection String");
            }

            var connectionString =
                    $"Host={host};Port={port};Database={db};Username={user};Password={password}";

            builder.Services.AddDbContext<FitnessTrackerDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddControllers()
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

            builder.Services
                .AddApplication()
                .AddRepositories();


            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler();

            app.MapControllers();

            app.Run();
        }
    }
}
