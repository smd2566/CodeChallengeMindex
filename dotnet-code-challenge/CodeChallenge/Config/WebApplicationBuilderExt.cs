using CodeChallenge.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeChallenge.Config
{
    public static class WebApplicationBuilderExt
    {
        private static readonly string DB_NAME_EMPLOYEE = "EmployeeDB";
        private static readonly string DB_NAME_COMPENSATION = "CompensationDB";
        
        /// <summary>
        /// Registers <see cref="EmployeeContext"/> backed by an in-memory database. Data lives only as long as the process
        /// and is shared by every context created with the same database name.
        /// </summary>
        public static void UseEmployeeDB(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<EmployeeContext>(options =>
            {
                options.UseInMemoryDatabase(DB_NAME_EMPLOYEE);
            });
        }
        //Creating a new database for the compensation functionality
        /// <summary>
        /// Registers <see cref="CompensationContext"/> backed by its own separate in-memory database,
        /// so compensation records are stored apart from employees.
        /// </summary>
        public static void UseCompensationDB(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<CompensationContext>(options =>
            {
                options.UseInMemoryDatabase(DB_NAME_COMPENSATION);
            });
        }
    }
}
