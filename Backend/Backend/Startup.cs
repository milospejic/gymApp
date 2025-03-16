using Backend.AuthHelp;
using Backend.Data.Context;
using Backend.Data.IRepository;
using Backend.Data.Repository;
using Backend.Entities;
using Backend.Utils;
using Backend.Utils.CustomExceptions;
using Backend.Utils.ExceptionHandlers;
using Backend.Utils.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace Backend
{
    /// <summary>
    /// The Startup class is responsible for configuring the services and the middleware pipeline for the application.
    /// This class sets up essential configurations like dependency injection, authentication, authorization,
    /// database context, and third-party libraries (like Swagger and AutoMapper).
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Gets the configuration settings for the application. This property is used to access various configuration values.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration settings from appsettings.json.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configures the services to be used by the application.
        /// This includes adding services like AutoMapper, database context, repositories, authentication, and Swagger.
        /// </summary>
        /// <param name="services">The service collection where services are registered.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Configures Cross-Origin Resource Sharing (CORS) policy to allow all origins, headers, and methods
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", builder =>
                {
                    builder.WithOrigins("http://localhost:5173") // Vite default port
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            // Configures the behavior for API controllers
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true; // Allows custom validation handling
            });

            // Adds Swagger/OpenAPI documentation for the API
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Gym Web API",
                    Description = "API to manage gym memberships",
                });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

                c.AddSecurityRequirement(securityRequirement);

                // Add XML documentation to Swagger UI
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // Add AutoMapper configuration for mapping DTOs to entities
            services.AddAutoMapper(typeof(Startup).Assembly);

            // Configures the DbContext for SQL Server
            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("GymDBConnection"))
                    .LogTo(Console.WriteLine, LogLevel.Information); // Logs SQL queries for debugging purposes
            });

            // JWT Authentication configuration
            var key = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidateLifetime = true
                };
            });

            // Adds authorization policies to require authentication with JWT Bearer token
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            // Register repositories for dependency injection
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<IMembershipPlanRepository, MembershipPlanRepository>();
            services.AddScoped<IAuthService, AuthService>();

            // Add custom exception handlers
            services.AddExceptionHandler<EmailAlreadyInUseExceptionHandler>();
            services.AddExceptionHandler<NotFoundExceptionHandler>();
            services.AddExceptionHandler<BadRequestExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();

            // Add problem details for standardized API error responses
            services.AddProblemDetails();
        }

        /// <summary>
        /// Configures the application's middleware pipeline.
        /// This includes setting up middleware for error handling, authentication, routing, and CORS.
        /// </summary>
        /// <param name="app">The application builder to configure middleware.</param>
        /// <param name="env">The environment the application is running in (e.g., Development, Production).</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Enable Swagger UI only in the development environment
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable HTTPS redirection
            app.UseHttpsRedirection();

            app.UseRouting(); // Set up routing middleware

            // Enable CORS policy
            app.UseCors("AllowFrontend");

            // Enable authentication middleware to process JWT Bearer tokens
            app.UseAuthentication();

            // Enable global exception handler
            app.UseExceptionHandler();

            // Enable authorization middleware for access control
            app.UseAuthorization();

            // Map controllers to the HTTP request pipeline
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Registers all controllers with the endpoint routing system
            });
        }
    }
}
