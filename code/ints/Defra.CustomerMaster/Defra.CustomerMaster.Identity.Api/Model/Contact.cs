using System;
using System.Net;
using System.Runtime.Serialization;

namespace Defra.CustomerMaster.Identity.Api.Model
{
    [DataContract]
    public class Contact
    {
        [DataMember]
        public string contactid;

        [DataMember]
        public string firstname;

        [DataMember]
        public string lastname;

        [DataMember]
        public string emailid;

        [DataMember]
        public string upn;

        [DataMember]
        public int Code;

        [DataMember]
        public string Message;

        [DataMember]
        public string MessageDetail;

        [DataMember]
        public HttpStatusCode HttpStatusCode;

    }
   
}