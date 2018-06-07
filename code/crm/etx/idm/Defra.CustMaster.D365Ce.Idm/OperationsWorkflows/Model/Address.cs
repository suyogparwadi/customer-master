using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Customer.Plugins.Model
{
    [DataContract]
    public class Address
    {
        [DataMember]
        public string type;
        [DataMember]
        public string uprn;
        [DataMember]
        public string buildingnumber;
        [DataMember]
        public string buildingname;
        [DataMember]
        public string street;
        [DataMember]
        public string locality;
        [DataMember]
        public string town;
        [DataMember]
        public string postcode;
        [DataMember]
        public string county;
        [DataMember]
        public string fromcompanieshouse;       
    }
   
}
