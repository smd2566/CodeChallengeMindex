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
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            //The below line was causing the bug where the directreports returned null. Instead of iterating through the method group
            //using SingleOrDefault, I instead used a foreeach loop which returned the proper values within the Employee object

            //return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);

            var returnedEmployee = new Employee();

            foreach (var employee in _employeeContext.Employees)
            {
                if (employee.EmployeeId == id)
                {
                    returnedEmployee = employee;
                
                }
                    
            }

            return returnedEmployee;
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}
