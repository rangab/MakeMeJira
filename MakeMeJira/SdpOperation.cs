using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MakeMeJira
{
    [XmlRoot("operation")]
    public class SdpOperation
    {
        [XmlElement("result")]
        public SdpResult Result { get; set; }

        [XmlElement("Details")]
        public SdpDetails Details { get; set; }
    }
}
