using System.Xml.Serialization;

namespace Chunithm.Tools.Validator.XMLClasses
{
    [XmlRoot(ElementName = "ddsBannerName")]
    public class DdsBannerName
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "flag")]
    public class Flag
    {
        [XmlElement(ElementName = "resourceVersion")]
        public ResourceVersion ResourceVersion { get; set; }
        [XmlElement(ElementName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "mapFilterID")]
    public class MapFilterID
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "courseNames")]
    public class CourseNames
    {
        [XmlElement(ElementName = "list")]
        public List List { get; set; }
    }

    [XmlRoot(ElementName = "image")]
    public class Image
    {
        [XmlElement(ElementName = "path")]
        public string Path { get; set; }
    }

    [XmlRoot(ElementName = "movieName")]
    public class MovieName
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "presentNames")]
    public class PresentNames
    {
        [XmlElement(ElementName = "list")]
        public List List { get; set; }
    }

    [XmlRoot(ElementName = "information")]
    public class Information
    {
        [XmlElement(ElementName = "resourceVersion")]
        public ResourceVersion ResourceVersion { get; set; }
        [XmlElement(ElementName = "informationType")]
        public string InformationType { get; set; }
        [XmlElement(ElementName = "informationDispType", IsNullable = true)]
        public string InformationDispType { get; set; }
        [XmlElement(ElementName = "mapFilterID")]
        public MapFilterID MapFilterID { get; set; }
        [XmlElement(ElementName = "courseNames")]
        public CourseNames CourseNames { get; set; }
        [XmlElement(ElementName = "text")]
        public string Text { get; set; }
        [XmlElement(ElementName = "image")]
        public Image Image { get; set; }
        [XmlElement(ElementName = "movieName")]
        public MovieName MovieName { get; set; }
        [XmlElement(ElementName = "presentNames")]
        public PresentNames PresentNames { get; set; }
    }

    [XmlRoot(ElementName = "mapName")]
    public class MapName
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "musicNames")]
    public class MusicNames
    {
        [XmlElement(ElementName = "list")]
        public List List { get; set; }
    }

    [XmlRoot(ElementName = "map")]
    public class Map
    {
        [XmlElement(ElementName = "resourceVersion")]
        public ResourceVersion ResourceVersion { get; set; }
        [XmlElement(ElementName = "tagText")]
        public string TagText { get; set; }
        [XmlElement(ElementName = "mapName")]
        public MapName MapName { get; set; }
        [XmlElement(ElementName = "musicNames")]
        public MusicNames MusicNames { get; set; }
    }

    [XmlRoot(ElementName = "StringID")]
    public class StringID
    {
        [XmlElement(ElementName = "id")]
        public int Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "music")]
    public class Music
    {
        [XmlElement(ElementName = "resourceVersion")]
        public ResourceVersion ResourceVersion { get; set; }
        [XmlElement(ElementName = "musicType")]
        public string MusicType { get; set; }
        [XmlElement(ElementName = "musicNames")]
        public MusicNames MusicNames { get; set; }
    }

    [XmlRoot(ElementName = "firstMovieName")]
    public class FirstMovieName
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "secondMovieName")]
    public class SecondMovieName
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "advertiseMovie")]
    public class AdvertiseMovie
    {
        [XmlElement(ElementName = "resourceVersion")]
        public ResourceVersion ResourceVersion { get; set; }
        [XmlElement(ElementName = "firstMovieName")]
        public FirstMovieName FirstMovieName { get; set; }
        [XmlElement(ElementName = "secondMovieName")]
        public SecondMovieName SecondMovieName { get; set; }
    }

    [XmlRoot(ElementName = "recommendMusic")]
    public class RecommendMusic
    {
        [XmlElement(ElementName = "resourceVersion")]
        public ResourceVersion ResourceVersion { get; set; }
        [XmlElement(ElementName = "musicNames")]
        public MusicNames MusicNames { get; set; }
    }

    [XmlRoot(ElementName = "release")]
    public class Release
    {
        [XmlElement(ElementName = "resourceVersion")]
        public ResourceVersion ResourceVersion { get; set; }
        [XmlElement(ElementName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "course")]
    public class Course
    {
        [XmlElement(ElementName = "resourceVersion")]
        public ResourceVersion ResourceVersion { get; set; }
        [XmlElement(ElementName = "courseNames")]
        public CourseNames CourseNames { get; set; }
    }

    [XmlRoot(ElementName = "questNames")]
    public class QuestNames
    {
        [XmlElement(ElementName = "list")]
        public List List { get; set; }
    }

    [XmlRoot(ElementName = "quest")]
    public class Quest
    {
        [XmlElement(ElementName = "resourceVersion")]
        public ResourceVersion ResourceVersion { get; set; }
        [XmlElement(ElementName = "questNames")]
        public QuestNames QuestNames { get; set; }
    }

    [XmlRoot(ElementName = "duelName")]
    public class DuelName
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "str")]
        public string Str { get; set; }
        [XmlElement(ElementName = "data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "duel")]
    public class Duel
    {
        [XmlElement(ElementName = "resourceVersion")]
        public ResourceVersion ResourceVersion { get; set; }
        [XmlElement(ElementName = "duelName")]
        public DuelName DuelName { get; set; }
    }

    [XmlRoot(ElementName = "changeSurfBoardUI")]
    public class ChangeSurfBoardUI
    {
        [XmlElement(ElementName = "resourceVersion")]
        public ResourceVersion ResourceVersion { get; set; }
        [XmlElement(ElementName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "substances")]
    public class Substances
    {
        [XmlElement(ElementName = "resourceVersion")]
        public ResourceVersion ResourceVersion { get; set; }
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "flag")]
        public Flag Flag { get; set; }
        [XmlElement(ElementName = "information")]
        public Information Information { get; set; }
        [XmlElement(ElementName = "map")]
        public Map Map { get; set; }
        [XmlElement(ElementName = "music")]
        public Music Music { get; set; }
        [XmlElement(ElementName = "advertiseMovie")]
        public AdvertiseMovie AdvertiseMovie { get; set; }
        [XmlElement(ElementName = "recommendMusic")]
        public RecommendMusic RecommendMusic { get; set; }
        [XmlElement(ElementName = "release")]
        public Release Release { get; set; }
        [XmlElement(ElementName = "course")]
        public Course Course { get; set; }
        [XmlElement(ElementName = "quest")]
        public Quest Quest { get; set; }
        [XmlElement(ElementName = "duel")]
        public Duel Duel { get; set; }
        [XmlElement(ElementName = "changeSurfBoardUI", IsNullable = true)]
        public ChangeSurfBoardUI ChangeSurfBoardUI { get; set; }
    }

    [XmlRoot(ElementName = "EventData")]
    public class EventData
    {
        [XmlElement(ElementName = "dataName")]
        public string DataName { get; set; }
        [XmlElement(ElementName = "formatVersion")]
        public string FormatVersion { get; set; }
        [XmlElement(ElementName = "resourceVersion")]
        public ResourceVersion ResourceVersion { get; set; }
        [XmlElement(ElementName = "name")]
        public Name Name { get; set; }
        [XmlElement(ElementName = "text")]
        public string Text { get; set; }
        [XmlElement(ElementName = "ddsBannerName")]
        public DdsBannerName DdsBannerName { get; set; }
        [XmlElement(ElementName = "periodDispType")]
        public string PeriodDispType { get; set; }
        [XmlElement(ElementName = "alwaysOpen")]
        public string AlwaysOpen { get; set; }
        [XmlElement(ElementName = "isAou")]
        public string IsAou { get; set; }
        [XmlElement(ElementName = "teamOnly", IsNullable = true)]
        public string TeamOnly { get; set; }
        [XmlElement(ElementName = "isKop")]
        public string IsKop { get; set; }
        [XmlElement(ElementName = "priority")]
        public string Priority { get; set; }
        [XmlElement(ElementName = "substances")]
        public Substances Substances { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
    }

}
