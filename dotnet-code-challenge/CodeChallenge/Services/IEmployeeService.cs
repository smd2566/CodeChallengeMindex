using CodeChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Services
{
    /// <summary>Employee operations used by the controllers. Implementations also save changes to the database.</summary>
    public interface IEmployeeService
    {
        /// <summary>Returns the employee with this id, or null if not found.</summary>
        Task<Employee> GetByIdAsync(String id);
        /// <summary>Saves a new employee (a new id is assigned) and returns it.</summary>
        Task<Employee> CreateAsync(Employee employee);
        /// <summary>Replaces originalEmployee with newEmployee, which keeps the original's id.</summary>
        Task<Employee> ReplaceAsync(Employee originalEmployee, Employee newEmployee);
    }
}
