using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Defra.CustomerMaster.Identity.Api
{
    [ServiceContract]
    public interface IEnrolmentAPI
    {

        [OperationContract]
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "/InitialMatch/{UPN}")]
        string InitialMatch(string UPN);


        [OperationContract]
        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "/UserInfo")]
        string UserInfo();

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }


    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
