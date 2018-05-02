using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Defra.CustomerMaster.Identity.Api
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

        

    }
}