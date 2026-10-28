using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    /// <summary>
    /// Business logic for employees. Sits between the controller and the repository; it guards against null/empty input
    /// and makes sure changes are saved after the repository stages them.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        /// <summary>Adds the employee to the database and saves. The repository assigns the new EmployeeId. Null input is ignored.</summary>
        public async Task<Employee> CreateAsync(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                await _employeeRepository.SaveAsync();
            }

            return employee;
        }

        /// <summary>Looks up an employee by id. Returns null for a null/empty id or when no employee matches.</summary>
        public async Task<Employee> GetByIdAsync(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return await _employeeRepository.GetByIdAsync(id);
            }

            return null;
        }

        /// <summary>
        /// Replaces an employee by deleting the original and inserting the new one under the original's id.
        /// If newEmployee is null the original is simply deleted. Does nothing if originalEmployee is null.
        /// </summary>
        public async Task<Employee> ReplaceAsync(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    await _employeeRepository.SaveAsync();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                await _employeeRepository.SaveAsync();
            }

            return newEmployee;
        }
    }
}
