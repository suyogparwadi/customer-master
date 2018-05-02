using Defra.CustomerMaster.Identity.Api.Dynamics;
using Newtonsoft.Json;
using System;

namespace Defra.CustomerMaster.Identity.Api
{

    public class EnrolmentAPI : IEnrolmentAPI
    {
        public string InitialMatch(string UPN)
        {
            try
            {
                //return string.Format("ServicieUserID is: {0}", new CrmApiWrapper().InitialMatch(UPN));

                Contact crmContact = new CrmApiWrapper().InitialMatch(UPN);
                ReturnObject returnObj = new ReturnObject() { ServiceUserID = crmContact.contactid.ToString() };

                //return string.Format1("ServicieU1serID is: {0}", );
                return JsonConvert.SerializeObject(returnObj);
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new ReturnObject() { ServiceUserID = null });
            }
        }

        public string UserInfo(Contact contact)
        {
            
            try
            {
                //Contact contactRequest = new Contact() { firstname = "test", lastName = "test", emailid = "testfromwcf@test.com2" };
                Contact crmContact = new CrmApiWrapper().UserInfo(contact);
                ReturnObject returnObj = new ReturnObject() { ServiceUserID = crmContact.contactid.ToString() };

                //return string.Format1("ServicieU1serID is: {0}", );
                return JsonConvert.SerializeObject(returnObj);
                
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ReturnObject() { ServiceUserID = null });
            }
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
