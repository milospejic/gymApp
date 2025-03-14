using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Backend
{
    /// <summary>
    /// The entry point for the application.
    /// This class is responsible for configuring and running the ASP.NET Core web host.
    /// It sets up the application to use the Startup class for configuration and initialization.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point of the application.
        /// It builds and runs the host for the ASP.NET Core application.
        /// </summary>
        /// <param name="args">Command line arguments passed to the application.</param>
        public static void Main(string[] args)
        {
            // Builds and runs the host
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates a host builder that configures the web host for the application.
        /// This method is called by the Main method to set up and configure the ASP.NET Core application.
        /// </summary>
        /// <param name="args">Command line arguments passed to the application.</param>
        /// <returns>An <see cref="IHostBuilder"/> that is used to configure the web host.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Specifies the Startup class for configuration
                    webBuilder.UseStartup<Startup>();
                });
    }
}
