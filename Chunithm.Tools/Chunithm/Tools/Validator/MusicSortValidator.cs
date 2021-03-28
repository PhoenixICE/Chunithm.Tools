using Chunithm.Tools.Validator.XMLClasses;
using Chunithm.Tools.Interface;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Chunithm.Tools.Validator
{
    public class MusicSortValidator : IFolderValidator
    {
        private readonly string _musicSortXML = "MusicSort.xml";
        private SerializeSortData musicSort;

        public IEnumerable<string> ValidateFolder(IEnumerable<OptionSubFolder> optionSubFolders)
        {
            var log = new List<string>();
            Program.LogInfo("Checking MusicSort.xml Validation...");
            var path = Path.Combine(InteractiveConsole.PreProcessing.Folders[FolderType.A000].FullName, OptionSubFolderType.music.ToString(), _musicSortXML);
            if (!System.IO.File.Exists(path))
            {
                log.Add("Missing MusicSort.xml in A000 Folder?");
                return log;
            }

            var xmlStr = System.IO.File.ReadAllText(path);

            var serializer = new XmlSerializer(typeof(SerializeSortData));
            using (var reader = new StringReader(xmlStr))
            {
                try
                {
                    musicSort = (SerializeSortData)serializer.Deserialize(reader);
                }
                catch
                {
                    log.Add($"Cannot phrase XML, {path} is potentially corrupted?");
                    return log;
                }
            }

            var musicSortIds = musicSort.SortList.StringID.Select(x => x.Id.ToString().PadLeft(4, '0')).Distinct();
            var optionMusic = InteractiveConsole.PreProcessing.Options.Where(x => x.Value.OptionSubFolders.ContainsKey(OptionSubFolderType.music)).SelectMany(x => x.Value.OptionSubFolders[OptionSubFolderType.music]?.Select(y => y.DirectoryInfo.Name.Substring(5)));

            var optionMusicNotInMusicSort = optionMusic.Except(musicSortIds);
            //var musicSortNotInOptionMusic = musicSortIds.Except(optionMusic);
            var total = optionMusic.Intersect(musicSortIds);

            foreach (var obj in optionMusicNotInMusicSort)
            {
                log.Add($"ID: {obj} not found in MusicSort.xml!");
            }

            log.Add($"Total Errors {optionMusicNotInMusicSort.Count()}");
            log.Add($"Total {total.Count()}");

            return log;
        }
    }
}