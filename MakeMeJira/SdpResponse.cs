using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MakeMeJira
{
    [XmlRoot("response")]
    public class SdpResponse
    {
       [XmlElement("operation")]
        public SdpOperation Operation { get; set; }
    }
}
