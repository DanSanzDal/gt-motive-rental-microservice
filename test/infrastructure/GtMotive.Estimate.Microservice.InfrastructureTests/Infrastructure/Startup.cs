using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using GtMotive.Estimate.Microservice.Api;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands;
using GtMotive.Estimate.Microservice.Infrastructure;
using GtMotive.Estimate.Microservice.Infrastructure.Configuration;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure
{
    public class Startup(IWebHostEnvironment environment, IConfiguration configuration)
    {
        public IWebHostEnvironment Environment { get; } = environment;

        public IConfiguration Configuration { get; } = configuration;

        public static void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // MediatR
            services.AddMediatR(typeof(CreateRentalCommand));

            services.AddAuthentication(TestServerDefaults.AuthenticationScheme)
                .AddTestServer();

            services.AddApiServices();

            // Configuración explícita de MongoDB para tests
            services.Configure<MongoDbSettings>(options =>
            {
                options.ConnectionString = "mongodb://localhost:27017";
                options.DatabaseName = "RentalTestDb";
            });

            services.AddInfrastructure(Configuration);
        }
    }
}
