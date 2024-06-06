using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class ReportingStructureControllerTests
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
        public void GetReportingStructureById_Returns_CorrectNumberOfReports()
        {
            //Making each variable explicit and readable

            var johnLennonId = "16a596ae-edd3-4847-99fe-c4518e82c86f"; //4 direct reports
            var ringoStarrId = "03aa1462-ffa9-4978-901b-7c001562cf6f"; //2 direct reports
            var peteBestId = "62c1084e-6e34-4630-93fd-9153afb65309"; //0 direct reports


            var getRequestTaskJohnLennon = _httpClient.GetAsync($"api/reportingStructure/{johnLennonId}");
            var getRequestTaskRingoStarr = _httpClient.GetAsync($"api/reportingStructure/{ringoStarrId}");
            var getRequestTaskPeteBest = _httpClient.GetAsync($"api/reportingStructure/{peteBestId}");

            var responseJohnLennon = getRequestTaskJohnLennon.Result;
            var responseRingoStarr = getRequestTaskRingoStarr.Result;
            var responsePeteBest = getRequestTaskPeteBest.Result;

            var reportingStructureJohnLennon = responseJohnLennon.DeserializeContent<ReportingStructure>();
            var reportingStructureRingoStarr = responseRingoStarr.DeserializeContent<ReportingStructure>();
            var reportingStructurePeteBest = responsePeteBest.DeserializeContent<ReportingStructure>();

            //Asserting that the returned result matches the expected result

            Assert.AreEqual(reportingStructureJohnLennon.NumberOfReports, 4);
            Assert.AreEqual(reportingStructureRingoStarr.NumberOfReports, 2);
            Assert.AreEqual(reportingStructurePeteBest.NumberOfReports, 0);


            
        }
    }
}
