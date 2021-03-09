using System.Xml.Serialization;
using System.Collections.Generic;

namespace Chunithm.Tools.Validator.MusicSortXML
{
    [XmlRoot(ElementName = "resourceVersion")]
    public class ResourceVersion
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "StringID")]
    public class StringID
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "SortList")]
    public class SortList
    {
        [XmlElement(ElementName = "StringID")]
        public List<StringID> StringID { get; set; }
    }

    [XmlRoot(ElementName = "SerializeSortData")]
    public class SerializeSortData
    {
        [XmlElement(ElementName = "dataName")]
        public string DataName { get; set; }
        [XmlElement(ElementName = "formatVersion")]
        public string FormatVersion { get; set; }
        [XmlElement(ElementName = "resourceVersion")]
        public ResourceVersion ResourceVersion { get; set; }
        [XmlElement(ElementName = "SortList")]
        public SortList SortList { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
    }
}