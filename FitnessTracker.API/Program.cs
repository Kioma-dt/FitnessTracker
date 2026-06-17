using FitnessTracker.API.ExceptionHandler;
using FitnessTracker.Application;
using FitnessTracker.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

            builder.Services.AddControllers();

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
            app.UseStatusCodePages();

            app.MapControllers();

            app.Run();
        }
    }
}
