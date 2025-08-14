using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IRI.Maptor.Ket.PersonalGdbPersistence.Xml;

[Serializable]
[XmlType("ArrayOfCodedValue", Namespace = "http://www.esri.com/schemas/ArcGIS/10.8")]
public class GdbXml_ArrayOfCodedValue
{
    [XmlAttribute("type", Namespace = System.Xml.Schema.XmlSchema.InstanceNamespace)]
    public string XsiType { get; set; } = "typens:ArrayOfCodedValue";


    [XmlElement("CodedValue", Namespace = "")]
    public List<GdbXml_CodedValue>? Items { get; set; }
}