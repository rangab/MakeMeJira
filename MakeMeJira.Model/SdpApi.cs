using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MakeMeJira
{
    [XmlRoot(ElementName = "API", DataType = "string", IsNullable=true)]
    public class SdpApi
    {
        //[XmlAttribute( AttributeName = "version")]
        //public string Version { get; set; }

        [XmlElement("response")]
        public SdpResponse Response { get; set; }
    }
}
