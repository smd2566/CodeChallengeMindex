using System;

using CodeChallenge.Data;
using CodeChallenge.Repositories;
using CodeChallenge.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeChallenge.Config
{
    /// <summary>
    /// Composition root: wires together the databases, services, repositories and request pipeline.
    /// </summary>
    public class App
    {
        /// <summary>
        /// Creates the web application: registers both in-memory databases and all services,
        /// seeds employee data (Development environment only), and maps the API controllers.
        /// </summary>
        public WebApplication Configure(string[] args)
        {
            args ??= Array.Empty<string>();

            var builder = WebApplication.CreateBuilder(args);

            builder.UseEmployeeDB();
            //Adding another builder db usage statement
            builder.UseCompensationDB();
            
            AddServices(builder.Services);

            var app = builder.Build();

            var env = builder.Environment;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedEmployeeDB();
            }

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        /// <summary>
        /// Registers the dependency injection bindings. The layering is Controller -> Service -> Repository -> DbContext,
        /// and each layer only depends on the interface of the layer below it. Scoped means one instance per HTTP request.
        /// </summary>
        private void AddServices(IServiceCollection services)
        {

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeRepository, EmployeeRespository>();
            //Adding additional services here for the created repositories in the persistance layer
            services.AddScoped<ICompensationService, CompensationService>();
            services.AddScoped<ICompensationRepository, CompensationRepository>();

            services.AddControllers();
        }

        /// <summary>
        /// Loads the employees from resources/EmployeeSeedData.json into the "EmployeeDB" in-memory database.
        /// The database name must match the one in <see cref="WebApplicationBuilderExt"/> so the seeder and the app share the same data.
        /// Only the employee database is seeded; the compensation database starts empty.
        /// </summary>
        private void SeedEmployeeDB()
        {
            new EmployeeDataSeeder(
                new EmployeeContext(
                    new DbContextOptionsBuilder<EmployeeContext>().UseInMemoryDatabase("EmployeeDB").Options
            )).Seed().Wait();
        }
    }
}
