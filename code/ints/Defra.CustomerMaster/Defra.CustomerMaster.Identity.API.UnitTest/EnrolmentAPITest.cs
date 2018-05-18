using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System.Threading;
using Newtonsoft.Json;
using Defra.CustomerMaster.Identity.Api;
using Defra.CustomerMaster.Identity.Api.Model;

namespace Defra.CustomerMaster.Identity.API.UnitTest
{
    [TestClass]
    public class EnrolmentAPITest
    {
        string Url = "http://localhost:3409/EnrolmentAPI.svc/";//"https://defra-custmast-dev.crm4.dynamics.com/";


        [TestMethod]
        public void InitialMatchTest()
        {

            string serviceID = "ab5fb791-624c-e811-a834-000d3a2b2be0";
            var client = new RestClient(Url);
            // var key = ApplicationAuthenticator.GetS2SAccessTokenForProdMSAAsync();
            var request = new RestRequest("InitialMatch/123", Method.GET);


            request.AddHeader("Accept", "*/*");
            request.AddHeader("Content-Type", "application/json");
            var action = new Action<IRestResponse<object>, RestRequestAsyncHandle>(CallbackResponse);


            var response = client.Get(request);
            Thread.Sleep(1000);
            if (!string.IsNullOrEmpty(response.Content))
            {
                string content = response.Content.Remove(0, 1);
                content = content.Remove(content.Length - 1, 1);
                content = content.Replace("\\", "");
                ServiceObject returnValue = JsonConvert.DeserializeObject<ServiceObject>(content);
                Assert.AreEqual(serviceID, returnValue.ServiceUserID);
            }


        }

        [TestMethod]
        public void UserInfoTest()
        {
            string serviceID = "2e0c1b70-f24d-e811-a83e-000d3a2b2ba3";
            var action = new Action<IRestResponse<object>, RestRequestAsyncHandle>(CallbackResponse);

            var client = new RestClient(Url);
            // var key = ApplicationAuthenticator.GetS2SAccessTokenForProdMSAAsync();
            var request = new RestRequest("UserInfo/", Method.PUT)
            {
                RequestFormat = DataFormat.Json

            };
            Contact contactContent = new Contact() { firstname = "testfirst", lastname = "testlast", emailid = "test0905200@test.com",upn= "123 - 123" };
            //string contactJson = JsonConvert.SerializeObject(contactContent);

            request.AddBody(contactContent);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Content-Type", "application/json");

            var response = client.Put(request);
            Thread.Sleep(1000);
            if (!string.IsNullOrEmpty(response.Content))
            {
                string content = response.Content.Remove(0, 1);
                content = content.Remove(content.Length - 1, 1).Replace("\\", "");

                ServiceObject returnValue = JsonConvert.DeserializeObject<ServiceObject>(content);
                Assert.AreEqual(serviceID, returnValue.ServiceUserID);
            }

        }

        private void CallbackResponse(IRestResponse<object> arg1, RestRequestAsyncHandle arg2)
        {
            throw new NotImplementedException();
        }
    }
}



