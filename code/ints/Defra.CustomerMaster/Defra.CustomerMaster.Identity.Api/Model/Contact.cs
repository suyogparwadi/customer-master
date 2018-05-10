using System;
using System.Runtime.Serialization;

namespace Defra.CustomerMaster.Identity.Api.Model
{
    [DataContract]
    public class Contact
    {
        [DataMember]
        public Nullable<Guid> contactid;

        [DataMember]
        public string firstname;

        [DataMember]
        public string lastname;

        [DataMember]
        public string emailid;

        [DataMember]
        public string UID;

    }
   
}