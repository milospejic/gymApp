using Backend.Data.Context;
using Backend.Data.IRepository;
using Backend.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Backend
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Add Swagger/OpenAPI
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Add AutoMapper
            services.AddAutoMapper(typeof(Startup).Assembly);

            // Add DbContext with SQL Server configuration
            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("GymDBConnection"))
                    .LogTo(Console.WriteLine, LogLevel.Information);
            });

            // Register repositories
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<IMembershipPlanRepository, MembershipPlanRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting(); // Ensure routing middleware is added

            app.UseAuthorization();

            // Map controllers using IEndpointRouteBuilder
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // This is where MapControllers() should be used
            });
        }
    }
}
