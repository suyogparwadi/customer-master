using System;
using System.Runtime.Serialization;
using System.Web;

namespace Defra.CustomerMaster.Identity.Api.Model
{
    [DataContract]
    public class AuthzResponse
    {
        [DataMember]
        public string version;

        [DataMember]
        public string status;

        [DataMember]
        public string roles;
    }
}