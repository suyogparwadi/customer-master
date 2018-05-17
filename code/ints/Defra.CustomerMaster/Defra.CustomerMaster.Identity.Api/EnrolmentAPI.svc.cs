using Defra.CustomerMaster.Identity.Api.Dynamics;
using Defra.CustomerMaster.Identity.Api.Model;
using Newtonsoft.Json;
using System;

namespace Defra.CustomerMaster.Identity.Api
{

    public class EnrolmentAPI : IEnrolmentAPI
    {
        public AuthzResponse Authz(string ServiceID, string UPN)
        {
            //return JsonConvert.SerializeObject(new AuthzResponse { status="200", version="1.0.0.0", roles = "role1:role2:role3:role4" });
            return new AuthzResponse { status = 200, version = "1.0.0.0", roles = "role1:role2:role3:role4" };
        }

        public string InitialMatch(string UPN)
        {
            try
            {
                System.Diagnostics.Trace.TraceError(UPN);
                //return string.Format("ServicieUserID is: {0}", new CrmApiWrapper().InitialMatch(UPN));
                if (string.IsNullOrEmpty(UPN) || string.IsNullOrWhiteSpace(UPN))
                    throw new ApplicationException("UPN can not be empty or null");
                Contact crmContact = new CrmApiWrapper().InitialMatch(UPN);
                ServiceObject returnObj = new ServiceObject() { ServiceUserID = crmContact.contactid.ToString() };

                //return string.Format1("ServicieU1serID is: {0}", );
                return JsonConvert.SerializeObject(returnObj);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ServiceObject() { ServiceUserID = null,ErrorMsg=ex.Message });
            }
        }

        public string UserInfo(Contact contact)
        {
            try
            {
                //Contact contactRequest = new Contact() { firstname = "test", lastName = "test", emailid = "testfromwcf@test.com2" };
                if((contact== null)|| string.IsNullOrEmpty(contact.upn))
                {
                    throw new ApplicationException("UPN can not be empty or null");
                }
                Contact crmContact = new CrmApiWrapper().UserInfo(contact);
                ServiceObject returnObj = new ServiceObject() { ServiceUserID = crmContact.contactid.ToString() };

                //return string.Format1("ServicieU1serID is: {0}", );
                return JsonConvert.SerializeObject(returnObj);

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ServiceObject() { ServiceUserID = null,ErrorMsg=ex.Message});
            }
        }
    }
}
