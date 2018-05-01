using Defra.CustomerMaster.Identity.Api.Dynamics;
using System;

namespace Defra.CustomerMaster.Identity.Api
{

    public class EnrolmentAPI : IEnrolmentAPI
    {
        public string InitialMatch(string UPN)
        {
            try
            {
                return string.Format("ContactId is: {0}", new CrmApiWrapper().InitialMatch(UPN));
            }
            catch(Exception ex)
            {
                return string.Format("ContactId is: {0}", ex.Message);
            }
        }

        public string UserInfo()
        {
            
            try
            {
                Contact contactRequest = new Contact() { firstName = "test", lastName = "test", emailid = "testfromwcf@test.com2" };

                return string.Format("ContactId is: {0}", new CrmApiWrapper().UserInfo(contactRequest));
                
            }
            catch (Exception ex)
            {
                return string.Format("ContactId is: {0}", ex.Message);
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
