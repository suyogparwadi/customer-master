using Defra.CustomerMaster.Identity.Api.Dynamics;
using Defra.CustomerMaster.Identity.Api.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace Defra.CustomerMaster.Identity.Api
{

    public class EnrolmentAPI : IEnrolmentAPI
    {
        public AuthzResponse Authz(string ServiceID, string UPN)
        {
            ServiceUserLinks serviceUserLinks = new CrmApiWrapper().Authz(ServiceID,UPN);
            //return JsonConvert.SerializeObject(serviceUserLinks.value);
            //return JsonConvert.SerializeObject(new AuthzResponse { status="200", version="1.0.0.0", roles = "role1:role2:role3:role4" });
            return new AuthzResponse { status = 200, version = "1.0.0.0",
                roles = new List<string>() { "ORG1GUID:Role1GUID", "ORG1GUID:Role2GUID", "ORG2GUID:Role21GUID" },
                mappings = new List<string>() { "ORG1GUID:ORG1NAME", "ORG2GUID:ORG2NAME", "Role1GUID:Role1Name", "Role2GUID:Role2Name", "Role21GUID:Role21Name" } };
        }

        public string InitialMatch(string UPN)
        {
            try
            {
                System.Diagnostics.Trace.TraceError("IntialMatch call:"+UPN);
                //return string.Format("ServicieUserID is: {0}", new CrmApiWrapper().InitialMatch(UPN));
                if (string.IsNullOrEmpty(UPN) || string.IsNullOrWhiteSpace(UPN))
                    throw new WebFaultException("UPN can not be empty or null",400);
                Contact crmContact = new CrmApiWrapper().InitialMatch(UPN);
                ServiceObject returnObj = new ServiceObject() { ServiceUserID = crmContact.contactid,ErrorCode=(int)crmContact.HttpStatusCode, ErrorMsg=crmContact.Message};

                //return string.Format1("ServicieU1serID is: {0}", );
                return JsonConvert.SerializeObject(returnObj);
            }
            catch (WebFaultException ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(new ServiceObject() { ServiceUserID = null, ErrorMsg = ex.ErrorMsg,ErrorCode=ex.HttpStatusCode});
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(new ServiceObject() { ServiceUserID = null,ErrorMsg=ex.Message });
            }
        }

        public string UserInfo(Contact contact)
        {
            try
            {
                System.Diagnostics.Trace.TraceError("UserInfo call params:{0},{1}",contact.UPN,contact.emailid);
                //Contact contactRequest = new Contact() { firstname = "test", lastName = "test", emailid = "testfromwcf@test.com2" };
                if ((contact== null)|| string.IsNullOrEmpty(contact.UPN))
                {
                    throw new WebFaultException("UPN can not be empty or null",401);
                }
                if ((contact == null) || string.IsNullOrEmpty(contact.lastname))
                {
                    throw new WebFaultException("lastname can not be empty or null", 401);
                }
                Contact crmContact = new CrmApiWrapper().UserInfo(contact);
                ServiceObject returnObj = new ServiceObject() { ServiceUserID = crmContact.contactid ,ErrorCode=(int)crmContact.HttpStatusCode, ErrorMsg=crmContact.Message };

                //return string.Format1("ServicieU1serID is: {0}", );
                return JsonConvert.SerializeObject(returnObj);

            }
            catch (WebFaultException ex)
            {                
                System.Diagnostics.Trace.TraceError(ex.Message);
                return JsonConvert.SerializeObject(new ServiceObject() { ServiceUserID = null, ErrorMsg = ex.ErrorMsg, ErrorCode = ex.HttpStatusCode });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                // throw new WebFaultException(customError, HttpStatusCode.NotFound);
                return JsonConvert.SerializeObject(new ServiceObject() { ServiceUserID = null, ErrorMsg = ex.Message });
            }
        }
    }
}
