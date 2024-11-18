using AgriBlazorServer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgriBlazorServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            // Register the IWeatherForecastService and its implementation
            services.AddScoped<IWeatherForecastService, WeatherForecastService>();

            // Add MVC services
            services.AddControllers();

            // Register HttpClient with custom handler
            services.AddHttpClient<WeatherForecastApiService>()
                    .ConfigurePrimaryHttpMessageHandler(() => new CustomHttpClientHandler());

            // Configure PostgreSQL connection
            // TODO: Replace the hard-coded values with environment variables
            var dbHost = "changeme";
            var dbPort = "changeme";
            var dbName = "changeme";
            var dbUser = "changeme";
            var dbPassword = "changeme";

            if (string.IsNullOrEmpty(dbHost) || string.IsNullOrEmpty(dbPort) || string.IsNullOrEmpty(dbName) || string.IsNullOrEmpty(dbUser) || string.IsNullOrEmpty(dbPassword))
            {
                throw new InvalidOperationException("Database connection information is missing. Please ensure all required environment variables are set.");
            }

            var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));
            // Test the database connection
            TestDatabaseConnection(connectionString);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");

                // Map controller routes
                endpoints.MapControllers();
            });
        }
        void TestDatabaseConnection(string connectionString)
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("Database connection successful.");
            }
            catch (NpgsqlException npgsqlEx)
            {
                Console.WriteLine($"PostgreSQL error: {npgsqlEx.Message}");
                throw new Exception($"Database connection failed due to PostgreSQL error: {npgsqlEx.Message}");
            }
            catch (InvalidOperationException invalidOpEx)
            {
                Console.WriteLine($"Invalid operation: {invalidOpEx.Message}");
                throw new Exception($"Database connection failed due to invalid operation: {invalidOpEx.Message}");
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine($"Argument error: {argEx.Message}");
                throw new Exception($"Database connection failed due to an argument error: {argEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                throw new Exception($"Database connection failed due to an unexpected error: {ex.Message}");
            }
        }
    }
}
