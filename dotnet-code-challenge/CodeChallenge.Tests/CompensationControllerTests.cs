using System;
using System.Net;
using System.Net.Http;
using System.Text;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Creating a fake employee based on EmployeeControllerTests.cs
            var employee = new Employee()
            {
                EmployeeId = Guid.NewGuid().ToString(), //unique per test so tests sharing the in-memory database do not collide <- AI bug fix
                Department = "Complaints",
                FirstName = "Debbie",
                LastName = "Downer",
                Position = "Receiver",
            };


            var compensation = new Compensation()
            {
                EmployeeId = employee.EmployeeId,
                Employee = employee,
                Salary = 75000.55m,
                EffectiveDate = "1/11/2000",

            };

            //Placing the compensation in the database

            var requestContentCompensation = new JsonSerialization().ToJson(compensation);

            var postRequestTaskCompensation = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContentCompensation, Encoding.UTF8, "application/json"));

            var responseCompensation = postRequestTaskCompensation.Result;

            Assert.AreEqual(HttpStatusCode.Created, responseCompensation.StatusCode);

            var newCompensation = responseCompensation.DeserializeContent<Compensation>();


            Assert.IsNotNull(newCompensation.Employee.EmployeeId);

            //Comparing the fields of the nested Employee object

            Assert.AreEqual(compensation.Employee.EmployeeId, newCompensation.Employee.EmployeeId);
            Assert.AreEqual(compensation.Employee.FirstName, newCompensation.Employee.FirstName);
            Assert.AreEqual(compensation.Employee.LastName, newCompensation.Employee.LastName);
            Assert.AreEqual(compensation.Employee.Department, newCompensation.Employee.Department);
            Assert.AreEqual(compensation.Employee.DirectReports, newCompensation.Employee.DirectReports);

            //Comparing the ReportingStructure exclusive fields
            Assert.AreEqual(compensation.EmployeeId, newCompensation.EmployeeId);
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
        }

        [TestMethod]
        public void CreateCompensation_Returns_Conflict_ForDuplicateEmployee()
        {
            var employee = new Employee()
            {
                EmployeeId = Guid.NewGuid().ToString(),
                Department = "Complaints",
                FirstName = "Debbie",
                LastName = "Downer",
                Position = "Receiver",
            };
            var compensation = new Compensation()
            {
                EmployeeId = employee.EmployeeId,
                Employee = employee,
                Salary = 75000.55m,
                EffectiveDate = "1/11/2000",
            };
            var requestContent = new JsonSerialization().ToJson(compensation);

            var first = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json")).Result;
            var second = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json")).Result;

            Assert.AreEqual(HttpStatusCode.Created, first.StatusCode);
            Assert.AreEqual(HttpStatusCode.Conflict, second.StatusCode);
        }

        [TestMethod]
        public void CreateCompensation_Returns_BadRequest_WhenEmployeeMissing()
        {
            //No nested employee and no employeeId
            var requestContent = "{ \"salary\": 75000.55, \"effectiveDate\": \"1/11/2000\" }";

            var response = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json")).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void GetCompensationById_Returns_NotFound()
        {
            var response = _httpClient.GetAsync($"api/compensation/{Guid.NewGuid()}").Result;

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void GetCompensationById_Returns_Ok()
        {

            // Creating a fake employee based on the EmployeeControllerTests.cs
            var employee = new Employee()
            {
                EmployeeId = Guid.NewGuid().ToString(), //unique per test so tests sharing the in-memory database do not collide <- AI bug fix
                Department = "Complaints",
                FirstName = "Debbie",
                LastName = "Downer",
                Position = "Receiver",
            };


            var compensation = new Compensation()
            {
                EmployeeId = employee.EmployeeId,
                Employee = employee,
                Salary = 75000.55m,
                EffectiveDate = "1/11/2000",

            };

            //Putting a compensation inside the database with an employee id, similar to the last test

            var requestContentCompensation = new JsonSerialization().ToJson(compensation);

            var postRequestTaskCompensation = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContentCompensation, Encoding.UTF8, "application/json"));

            var responseCompensation = postRequestTaskCompensation.Result;

            Assert.AreEqual(HttpStatusCode.Created, responseCompensation.StatusCode);

            var newCompensation = responseCompensation.DeserializeContent<Compensation>();


            // Arrange
            var employeeId = employee.EmployeeId;
            var expectedSalary = 75000.55m;
            var expectedEffectiveDate = "1/11/2000";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var returnedCompensation = response.DeserializeContent<Compensation>();

            //Employee does not override Equals, so compare its fields rather than the object references
            Assert.IsNotNull(returnedCompensation.Employee);
            Assert.AreEqual(employee.EmployeeId, returnedCompensation.Employee.EmployeeId);
            Assert.AreEqual(employee.FirstName, returnedCompensation.Employee.FirstName);
            Assert.AreEqual(employee.LastName, returnedCompensation.Employee.LastName);
            Assert.AreEqual(employeeId, returnedCompensation.EmployeeId);
            Assert.AreEqual(expectedSalary, returnedCompensation.Salary);
            Assert.AreEqual(expectedEffectiveDate, returnedCompensation.EffectiveDate);
        }

        
    }
}

