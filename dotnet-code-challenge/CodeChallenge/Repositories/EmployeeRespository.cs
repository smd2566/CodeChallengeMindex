using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    /// <summary>
    /// Data-access layer for employees, wrapping <see cref="EmployeeContext"/>. Add and Remove only stage changes;
    /// nothing is persisted until <see cref="SaveAsync"/> is called (the service does this).
    /// </summary>
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        /// <summary>Stages a new employee for insertion, overwriting its EmployeeId with a fresh GUID. Requires SaveAsync to persist.</summary>
        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        /// <summary>Returns the employee with the given id, or null if there is none (callers rely on null to produce a 404).</summary>
        public async Task<Employee> GetByIdAsync(string id)
        {
            //The below line was causing the bug where the directreports returned null. Instead of iterating through the method group
            //using SingleOrDefault, I instead used a foreeach loop which returned the proper values within the Employee object

            //return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);

            //Defaults to null so callers can detect an employee that does not exist <- AI fix
            Employee returnedEmployee = null;

            await foreach (var employee in _employeeContext.Employees.AsAsyncEnumerable())
            {
                if (employee.EmployeeId == id)
                {
                    returnedEmployee = employee;
                
                }
                    
            }

            return returnedEmployee;
        }

        /// <summary>Commits all staged changes (adds/removes) to the database.</summary>
        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        /// <summary>Stages an existing employee for deletion. The employee must have a non-null EmployeeId. Requires SaveAsync to persist.</summary>
        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}
