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
        
        public static void UseEmployeeDB(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<EmployeeContext>(options =>
            {
                options.UseInMemoryDatabase(DB_NAME_EMPLOYEE);
            });
        }
        //Creating a new database for the compensation functionality
        public static void UseCompensationDB(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<CompensationContext>(options =>
            {
                options.UseInMemoryDatabase(DB_NAME_COMPENSATION);
            });
        }
    }
}
