using System.Xml.Serialization;
using System.Collections.Generic;

namespace Chunithm.Tools.Validator.XMLClasses
{
    [XmlRoot(ElementName = "releaseTagName")]
    public class ReleaseTagName
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "netOpenName")]
    public class NetOpenName
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "name")]
    public class Name
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "rightsInfoName")]
    public class RightsInfoName
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "artistName")]
    public class ArtistName
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "list")]
    public class List
    {
        [XmlElement(ElementName = "StringID")]
        public StringID StringID { get; set; }
    }

    [XmlRoot(ElementName = "genreNames")]
    public class GenreNames
    {
        [XmlElement(ElementName = "list")]
        public List List { get; set; }
    }

    [XmlRoot(ElementName = "musicLableID")]
    public class MusicLableID
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "worksName")]
    public class WorksName
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "jaketFile")]
    public class JaketFile
    {
        [XmlElement(ElementName = "path")]
        public string Path { get; set; }
    }

    [XmlRoot(ElementName = "cueFileName")]
    public class CueFileName
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "worldsEndTagName")]
    public class WorldsEndTagName
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "stageName")]
    public class StageName
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "type")]
    public class Type
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "file")]
    public class File
    {
        [XmlElement(ElementName = "path")]
        public string Path { get; set; }
    }

    [XmlRoot(ElementName = "MusicFumenData")]
    public class MusicFumenData
    {
        [XmlElement(ElementName = "resourceVersion")]
        public ResourceVersion ResourceVersion { get; set; }
        [XmlElement(ElementName = "type")]
        public Type Type { get; set; }
        [XmlElement(ElementName = "enable")]
        public string Enable { get; set; }
        [XmlElement(ElementName = "file")]
        public File File { get; set; }
        [XmlElement(ElementName = "level")]
        public string Level { get; set; }
        [XmlElement(ElementName = "levelDecimal")]
        public string LevelDecimal { get; set; }
        [XmlElement(ElementName = "notesDesigner")]
        public string NotesDesigner { get; set; }
        [XmlElement(ElementName = "defaultBpm")]
        public string DefaultBpm { get; set; }
    }

    [XmlRoot(ElementName = "fumens")]
    public class Fumens
    {
        [XmlElement(ElementName = "MusicFumenData")]
        public List<MusicFumenData> MusicFumenData { get; set; }
    }

    [XmlRoot(ElementName = "MusicData")]
    public class MusicData
    {
        [XmlElement(ElementName = "dataName")]
        public string DataName { get; set; }
        [XmlElement(ElementName = "formatVersion")]
        public string FormatVersion { get; set; }
        [XmlElement(ElementName = "resourceVersion")]
        public ResourceVersion ResourceVersion { get; set; }
        [XmlElement(ElementName = "releaseTagName")]
        public ReleaseTagName ReleaseTagName { get; set; }
        [XmlElement(ElementName = "netOpenName")]
        public NetOpenName NetOpenName { get; set; }
        [XmlElement(ElementName = "disableFlag")]
        public string DisableFlag { get; set; }
        [XmlElement(ElementName = "exType")]
        public string ExType { get; set; }
        [XmlElement(ElementName = "name")]
        public Name Name { get; set; }
        [XmlElement(ElementName = "rightsInfoName")]
        public RightsInfoName RightsInfoName { get; set; }
        [XmlElement(ElementName = "sortName")]
        public string SortName { get; set; }
        [XmlElement(ElementName = "artistName")]
        public ArtistName ArtistName { get; set; }
        [XmlElement(ElementName = "genreNames")]
        public GenreNames GenreNames { get; set; }
        [XmlElement(ElementName = "musicLableID")]
        public MusicLableID MusicLableID { get; set; }
        [XmlElement(ElementName = "worksName")]
        public WorksName WorksName { get; set; }
        [XmlElement(ElementName = "jaketFile")]
        public JaketFile JaketFile { get; set; }
        [XmlElement(ElementName = "firstLock")]
        public string FirstLock { get; set; }
        [XmlElement(ElementName = "priority")]
        public string Priority { get; set; }
        [XmlElement(ElementName = "cueFileName")]
        public CueFileName CueFileName { get; set; }
        [XmlElement(ElementName = "previewStartTime")]
        public string PreviewStartTime { get; set; }
        [XmlElement(ElementName = "previewEndTime")]
        public string PreviewEndTime { get; set; }
        [XmlElement(ElementName = "worldsEndTagName", IsNullable = true)]
        public WorldsEndTagName WorldsEndTagName { get; set; }
        [XmlElement(ElementName = "starDifType")]
        public string StarDifType { get; set; }
        [XmlElement(ElementName = "stageName")]
        public StageName StageName { get; set; }
        [XmlElement(ElementName = "fumens")]
        public Fumens Fumens { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
    }
}
