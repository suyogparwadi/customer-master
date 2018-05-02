using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Defra.CustomerMaster.Identity.API.UnitTest
{
        [TestClass]
        public class UnitTest1
        {
            string Url = "https://defra-custmast-dev.crm4.dynamics.com/";
            string Username = "aruna.ramidi@defradev.onmicrosoft.com";
            string Password = "test";
            string ClientId = "1801d83b-54ff-4f63-bcfe-9133a4e168fc";

            [TestMethod]
            public void TestMethod1()
            {

            var client = new RestClient("http://localhost:3409/EnrolmentAPI.svc/");
            // var key = ApplicationAuthenticator.GetS2SAccessTokenForProdMSAAsync();
            var request = new RestRequest("InitialMatch/123", Method.GET);
            //{
            //    RequestFormat = DataFormat.J
            //};

            request.AddHeader("Accept", "*/*");
            request.AddHeader("Content-Type", "application/json");
            // request.AddHeader("Authorization", $"Bearer {AcquireToken()}");

            //request.AddBody("contact");

            var response = client.Get(request).Content;
            //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

    }
    }


