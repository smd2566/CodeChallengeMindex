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
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        
        [HttpGet("{employeeId}", Name = "getReportingStructureById")]
        public IActionResult GetReportingStructureById(String employeeId)
        {
            

            _logger.LogDebug($"Received reporting structure get request for '{employeeId}'");

            var reportingStructure = new ReportingStructure(); //creating a new ReportingStructureObject to be returned
            var employee = _employeeService.GetById(employeeId);

            

            if (employee == null)
                return NotFound();

            //Setting the values of the new object

            reportingStructure.Employee = employee;
            reportingStructure.NumberOfReports = TraverseDirectReports(employee);

            
            //Returning the newly completed object

            return Ok(reportingStructure);
        }

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
