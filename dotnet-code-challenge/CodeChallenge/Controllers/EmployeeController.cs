using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/employee")]
    /// <summary>
    /// HTTP endpoints for employees (api/employee). Controllers only handle HTTP concerns (status codes, request/response bodies)
    /// and delegate the actual work to <see cref="IEmployeeService"/>.
    /// </summary>
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        /// <summary>
        /// POST api/employee - creates an employee. Any EmployeeId in the request is ignored because the repository
        /// assigns a new GUID. Returns 201 with the saved employee and a Location header pointing at the GET endpoint.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            await _employeeService.CreateAsync(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        /// <summary>
        /// GET api/employee/{id} - returns the employee (including nested direct reports) with 200, or 404 if the id does not exist.
        /// The route name is referenced by <see cref="CreateEmployee"/> when building the Location header.
        /// </summary>
        [HttpGet("{id}", Name = "getEmployeeById")]
        public async Task<IActionResult> GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");



            var employee = await _employeeService.GetByIdAsync(id);


            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        /// <summary>
        /// PUT api/employee/{id} - replaces an existing employee with the request body. Returns 404 if {id} does not exist.
        /// The employee keeps its original id even if the body contains a different one.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceEmployee(String id, [FromBody]Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = await _employeeService.GetByIdAsync(id);
            if (existingEmployee == null)
                return NotFound();

            await _employeeService.ReplaceAsync(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }
    }
}
