using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MakeMeJira
{
    [XmlRoot("Details")]
    public class SdpDetails
    {
        [XmlElement("parameter")]
        public SdpParameter[] Parameters { get; set; } 
    }
}

