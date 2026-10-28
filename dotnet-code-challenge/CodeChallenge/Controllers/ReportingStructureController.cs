using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;
using System.Xml.Linq;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/reportingStructure")]
    /// <summary>
    /// Read-only endpoint (api/reportingStructure) for the Task 1 requirement: how many people report, directly or indirectly,
    /// to a given employee. Nothing is stored; the result is calculated on every request.
    /// </summary>
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        /// <summary>
        /// GET api/reportingStructure/{employeeId} - returns the employee together with the total number of reports beneath them.
        /// Returns 404 if the employee does not exist.
        /// </summary>
        [HttpGet("{employeeId}", Name = "getReportingStructureById")]
        public async Task<IActionResult> GetReportingStructureById(String employeeId)
        {
            

            _logger.LogDebug($"Received reporting structure get request for '{employeeId}'");

            var reportingStructure = new ReportingStructure(); //creating a new ReportingStructureObject to be returned
            var employee = await _employeeService.GetByIdAsync(employeeId);

            

            if (employee == null)
                return NotFound();

            //Setting the values of the new object

            reportingStructure.Employee = employee;
            reportingStructure.NumberOfReports = TraverseDirectReports(employee);

            
            //Returning the newly completed object

            return Ok(reportingStructure);
        }

        /// <summary>
        /// Counts everyone below the given employee: their direct reports plus, recursively, each of those people's reports.
        /// The employee themself is not counted. Employees with no direct reports (null or empty list) return 0.
        /// </summary>
        //I created a recursive helper method here to account for any number of employees that report to the base employee
        public static int TraverseDirectReports(Employee employee)
        {
            int reportCount = 0;
            
            if (employee.DirectReports != null)
            {
                //Adding the un-nested employees first
                reportCount += employee.DirectReports.Count;

                //Traversing through the nested employees
                foreach (Employee member in employee.DirectReports)
                {
                    reportCount += TraverseDirectReports(member);
                }
            }
            return reportCount;
        }
    }
}
