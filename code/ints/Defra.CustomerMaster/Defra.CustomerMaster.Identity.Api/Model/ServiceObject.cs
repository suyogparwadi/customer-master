using System;
using System.Runtime.Serialization;


namespace Defra.CustomerMaster.Identity.Api
{

    [DataContract]
    public class ServiceObject
    {
        [DataMember]
        public string ServiceUserID;
    }
}