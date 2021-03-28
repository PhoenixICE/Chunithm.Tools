using Chunithm.Tools.Interface;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using Chunithm.Tools.Validator.XMLClasses;

namespace Chunithm.Tools.Validator
{
    public class MusicDuplicatorValidator : IFolderValidator
    {
        public IEnumerable<string> ValidateFolder(IEnumerable<OptionSubFolder> optionSubFolders)
        {
            var log = new List<string>();
            Program.LogInfo("Checking music.xml Duplicate Validation...");

            var musics = new Dictionary<DirectoryInfo, MusicData>();
            var serializer = new XmlSerializer(typeof(MusicData));
            foreach (var music in optionSubFolders.Where(x => x.OptionType == OptionSubFolderType.music))
            {
                var musicFile = System.IO.File.ReadAllText(music.XMLFile.FullName);
                using (var reader = new StringReader(musicFile))
                {
                    try
                    {
                        var musicData = (MusicData)serializer.Deserialize(reader);
                        musics.Add(music.DirectoryInfo, musicData);
                    }
                    catch
                    {
                        log.Add($"Cannot phrase XML, {music.XMLFile.FullName} is potentially corrupted?");
                        continue;
                    }
                }
            }

            var regularSongs = musics.Where(x => x.Value.WorldsEndTagName == null);
            var worldEndSongs = musics.Where(x => x.Value.WorldsEndTagName != null);

            var potentialDupes = regularSongs.GroupBy(x => x.Value.Name.Str).Where(x => x.Count() > 1).Select(x => $"Duplicate Song: {x.Key}, {string.Join(", ", x)}");
            log.Add("============================================");
            var potentialWorldEndDupes = worldEndSongs.GroupBy(x => x.Value.Name.Str).Where(x => x.Count() > 1).Select(x => $"Duplicate WorldEnd: {x.Key} - {string.Join(", ", x.Select(y => $"{y.Key.Parent.Parent.Name}:{y.Value.DataName}"))}");
            log.AddRange(potentialDupes);
            log.AddRange(potentialWorldEndDupes);
            log.Add($"Total Potential Dupes: {potentialDupes.Count()}");
            log.Add($"Total Potential WorldEnd Dupes: {potentialWorldEndDupes.Count()}");
            log.Add("Remember these are potential duplicates based on song name!");
            return log;
        }
    }
}