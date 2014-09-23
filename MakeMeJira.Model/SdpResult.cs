using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MakeMeJira
{
    [XmlRoot("result")]
    public class SdpResult
    {
        [XmlElement("statuscode")]
        public string StatusCode { get; set; }

        [XmlElement("static")]
        public string Status { get; set; }

        [XmlElement("message")]
        public string Message { get; set; }


    }
}
