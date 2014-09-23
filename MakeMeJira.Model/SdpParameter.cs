using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MakeMeJira
{
    [XmlRoot("parameter")]
    public class SdpParameter
    {
         [XmlElement("name")]
        public string Name { get; set; }
        
        [XmlElement("value")]
        public string Value { get; set; }
    }
}
