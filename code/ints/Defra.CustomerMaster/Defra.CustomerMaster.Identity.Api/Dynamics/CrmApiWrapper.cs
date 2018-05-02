using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using Newtonsoft.Json;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace Defra.CustomerMaster.Identity.Api.Dynamics
{
    public class CrmApiWrapper
    {
        private HttpClient httpClient;
        private const string Authority = "https://login.windows.net/common";
        private readonly string _clientId;//= "d38f2cff-29d1-43f6-a3e8-4b91baf4b065";
        private readonly string _password;//= "Yizh1TCPXsOMvXbC8Duu";
        private readonly string _resource;// = "https://defra-custmast-dev.api.crm4.dynamics.com";
        private readonly string _username;// = "aruna.ramidi@defradev.onmicrosoft.com";

        public CrmApiWrapper()
        {

            if (!ConfigurationManager.AppSettings.AllKeys.ToList().Contains("DynamicsApiTokenUrl"))
            {
                throw new ApplicationException("There was no configuration item with the name DynamicsApiTokenUrl in the config");
            }
            if (!ConfigurationManager.AppSettings.AllKeys.ToList().Contains("DynamicsApiClientId"))
            {
                throw new ApplicationException("There was no configuration item with the name DynamicsApiClientId in the config");
            }
            if (!ConfigurationManager.AppSettings.AllKeys.ToList().Contains("DynamicsApiPassword"))
            {
                throw new ApplicationException("There was no configuration item with the name DynamicsApiPassword in the config");
            }
            if (!ConfigurationManager.AppSettings.AllKeys.ToList().Contains("DynamicsApiUsername"))
            {
                throw new ApplicationException("There was no configuration item with the name DynamicsApiUsername in the config");
            }
            _resource = ConfigurationManager.AppSettings["DynamicsApiTokenUrl"];
            _clientId = ConfigurationManager.AppSettings["DynamicsApiClientId"];
            _password = ConfigurationManager.AppSettings["DynamicsApiPassword"];
            _username = ConfigurationManager.AppSettings["DynamicsApiUsername"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UPN"></param>
        /// <returns></returns>       
        public string InitialMatchOld(string UPN)
        {
            if (string.IsNullOrEmpty(UPN))
            {
                throw new ApplicationException("UPN is not present in the request. Please provide the UPN");
            }
            //string[] uria = new string[] {
            //    "defra_InitialMatch",
            //    "(UPN=@p1)",
            //    "?@p1='" + UPN+"'"};
           

            JObject exeAction = new JObject();            
            exeAction["UPN"] = UPN;

            string paramsContent;
            if (exeAction.GetType().Name.Equals("JObject"))
            { paramsContent = exeAction.ToString(); }
            else
            {
                paramsContent = JsonConvert.SerializeObject(exeAction, new JsonSerializerSettings()
                { DefaultValueHandling = DefaultValueHandling.Ignore });
            }

            Configuration config = new Configuration();
            config.Username = _username;


            SecureString Password = new SecureString();
            foreach (char c in _password) Password.AppendChar(c);

            config.Password = Password;
            config.ClientId = _clientId;
            config.RedirectUrl = Authority;
            config.ServiceUrl = _resource;
            Authentication auth = new Authentication(config);//, Authority);  
            
            httpClient = new HttpClient(auth.ClientHandler, true);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, config.ServiceUrl + "api/data/v8.2/defra_InitialMatch");
            request.Content = new StringContent(paramsContent);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");


            //httpClient.BaseAddress = new Uri(config.ServiceUrl + "api/data/v8.2/");
            httpClient.Timeout = new TimeSpan(0, 2, 0);
            httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
         



            // httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {auth.AcquireToken().AccessToken}");


            // var contactResonse = httpClient.GetAsync("contacts").Result;
            //// httpClient.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations=OData.Community.Display.V1.FormattedValue");
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.AcquireToken().AccessToken);
            //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {auth.AcquireToken().AccessToken}");


            // var content = contactResonse.Result.Content.ReadAsStringAsync().Result;
            //var contactResonse = httpClient.GetAsync("contacts").Result;
            var contactResponse = httpClient.SendAsync(request).Result;
            //var contactResponse = httpClient.GetAsync("contacts?$select=contactid&$filter=defra_upn eq '"+UPN+"'").Result;
            //var contactResonse = httpClient.GetAsync(string.Join("", uria)).Result;
            if (!contactResponse.IsSuccessStatusCode)
            {
                return null;
            }
            var content = contactResponse.Content.ReadAsStringAsync().Result;

            
            var contact = JsonConvert.DeserializeObject<Contact>(content);
            return JsonConvert.SerializeObject(contact.contactid);
        }

        public Contact InitialMatch(string UPN)
        {
            JObject exeAction = new JObject();
            exeAction["UPN"] = UPN;

            string paramsContent;
            if (exeAction.GetType().Name.Equals("JObject"))
            { paramsContent = exeAction.ToString(); }
            else
            {
                paramsContent = JsonConvert.SerializeObject(exeAction, new JsonSerializerSettings()
                { DefaultValueHandling = DefaultValueHandling.Ignore });
            }
            //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _resource+ "api/data/v8.2/contacts?$select=contactid&$filter=defra_upn eq '"+UPN+"'");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _resource + "api/data/v8.2/defra_InitialMatch");
            request.Content = new StringContent(paramsContent);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            Contact jsonValue = ConnectToCRM(request);
            return jsonValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
       public Contact UserInfo(Contact contact)
        {        
            JObject exeAction = new JObject();
            exeAction["firstname"] = contact.firstname;
            exeAction["lastname"] = contact.lastname;
            exeAction["emailid"] = contact.emailid;

            string paramsContent;
            if (exeAction.GetType().Name.Equals("JObject"))
            { paramsContent = exeAction.ToString(); }
            else
            {
                paramsContent = JsonConvert.SerializeObject(exeAction, new JsonSerializerSettings()
                { DefaultValueHandling = DefaultValueHandling.Ignore });
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _resource + "api/data/v8.2/defra_UpsertContact");
            request.Content = new StringContent(paramsContent);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            Contact value=ConnectToCRM(request);

            return value;


        }
        private Contact ConnectToCRM(HttpRequestMessage request)
        {
            Configuration config = new Configuration();
            config.Username = _username;


            SecureString Password = new SecureString();
            foreach (char c in _password) Password.AppendChar(c);

            config.Password = Password;
            config.ClientId = _clientId;
            config.RedirectUrl = Authority;
            config.ServiceUrl = _resource;
            Authentication auth = new Authentication(config);//, Authority);  

            httpClient = new HttpClient(auth.ClientHandler, true);

           
            httpClient.Timeout = new TimeSpan(0, 2, 0);
            httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {auth.AcquireToken().AccessToken}");

            var contactResponse = httpClient.SendAsync(request).Result;

            if (!contactResponse.IsSuccessStatusCode)
            {
                return null;
            }
            var content = contactResponse.Content.ReadAsStringAsync().Result;


            var contact = JsonConvert.DeserializeObject<Contact>(content);
            return contact;         }
    }

}
