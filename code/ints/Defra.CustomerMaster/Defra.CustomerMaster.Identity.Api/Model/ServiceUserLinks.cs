using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Defra.CustomerMaster.Identity.Api.Model
{
    [DataContract]
    public class ServiceUserLink
    {
        [DataMember]
        public string createdon;

        [DataMember]
        public string defra_lobserviceuserlinkid;

        [DataMember]
        public string defra_name;

        [DataMember]
        public string _defra_serviceuser_value;

        [DataMember]
        public string defra_ServiceUser;

        [DataMember]
        public string _defra_servicerole_value;

        [DataMember]
        public string defra_ServiceRole;

        [DataMember]
        public string serviceLinkContact_x002e_fullname;

        [DataMember]
        public string serviceLinkRole_x002e_defra_lobserivceroleid;

        [DataMember]
        public string serviceLinkOrganisation_x002e_name;

        [DataMember]
        public string serviceLinkRole_x002e_defra_name;
    }

    [DataContract]
    public class ServiceUserLinks
    {
        [DataMember]
        public List<ServiceUserLink> value;
    }
}