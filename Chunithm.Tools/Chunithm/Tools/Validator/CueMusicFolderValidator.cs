using Chunithm.Tools.Interface;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Chunithm.Tools.Validator
{
    public class CueMusicFolderValidator : IFolderValidator
    {
        private readonly string[] _regularCharts =
        {
            "{0}_00.c2s",
            "{0}_01.c2s",
            "{0}_02.c2s",
            "{0}_03.c2s",
            "CHU_UI_Jacket_{0}.dds",
            "music.xml",
        };

        private readonly string[] _worldEndFiles =
        {
            "{0}_04.c2s",
            "CHU_UI_Jacket_",
            "music.xml",
        };

        private readonly string[] _cueMusicFiles =
        {
            "CueFile.xml",
            "music{0}.acb",
            "music{0}.awb",
        };

        private readonly string[] _cueSystemVoiceFiles =
        {
            "CueFile.xml",
            "systemvoice{0}.acb",
            "systemvoice{0}.awb",
        };

        public IEnumerable<string> ValidateFolder(IEnumerable<OptionSubFolder> optionSubFolders)
        {
            Program.LogInfo("Checking Chart/Song Validation...");
            var cueFileFolders = optionSubFolders.Where(x => x.OptionType == OptionSubFolderType.cueFile);
            var musicFolders = optionSubFolders.Where(x => x.OptionType == OptionSubFolderType.music);

            var worldEndFiles = new List<string>();
            var musicFiles = new List<string>();
            var cueFiles = new List<string>();

            var output = new List<string>();

            foreach (var cueFileFolder in cueFileFolders)
            {
                var directoryFiles = cueFileFolder.DirectoryInfo.GetFiles();
                var number = cueFileFolder.DirectoryInfo.Name.Substring(cueFileFolder.DirectoryInfo.Name.Length - 4, 4);

                var cueMusicFilesMissing = CheckMissingFiles(directoryFiles, number, _cueMusicFiles);
                if (cueMusicFilesMissing.Count != 0)
                {
                    var cueSystemVoiceFilesMissing = CheckMissingFiles(directoryFiles, number, _cueSystemVoiceFiles);
                    if (cueSystemVoiceFilesMissing.Count != 0)
                    {
                        output.Add($"Missing CueFiles for {cueFileFolder.DirectoryInfo.FullName} {string.Join(", ", cueMusicFilesMissing)}");
                    }
                }
                else
                {
                    cueFiles.Add(number);
                }
            }

            foreach (var musicFolder in musicFolders)
            {
                var directoryFiles = musicFolder.DirectoryInfo.GetFiles();
                var number = musicFolder.DirectoryInfo.Name.Substring(musicFolder.DirectoryInfo.Name.Length - 4, 4);

                var chartFilesMissing = CheckMissingFiles(directoryFiles, number, _regularCharts);
                if (chartFilesMissing.Count != 0)
                {
                    var worldEndFilesMissing = CheckMissingFiles(directoryFiles, number, _worldEndFiles);
                    if (worldEndFilesMissing.Count != 0)
                    {
                        output.Add($"Missing MusicFiles for {musicFolder.DirectoryInfo.FullName} {string.Join(", ", chartFilesMissing)}");
                    }
                    else
                    {
                        worldEndFiles.Add(number);
                    }
                }
                else
                {
                    musicFiles.Add(number);
                }
            }

            var missingCueFiles = musicFiles.Distinct().Except(cueFiles);
            var missingMusicFiles = cueFiles.Distinct().Except(musicFiles);

            foreach (var missingCueFile in missingCueFiles)
            {
                output.Add($"Music File {missingCueFile} exists but not Cue File!");
            }

            foreach (var missingMusicFile in missingMusicFiles)
            {
                output.Add($"Cue File {missingMusicFile} exists but not Music File!");
            }

            output.Add($"Total Errors: {missingCueFiles.Count() + missingMusicFiles.Count()}");
            output.Add($"Total Valid Cue+Music Files: {musicFiles.Intersect(cueFiles).Count()}");
            output.Add($"Total World End Files: {worldEndFiles.Distinct().Count()}");

            output.Add("Checking Chart/Song Validation Completed.");
            return output;
        }

        private List<string> CheckMissingFiles(FileInfo[] directoryFiles, string number, string[] filesToCheck)
        {
            var missing = new List<string>();
            foreach (var file in filesToCheck)
            {
                var fileFormatted = string.Format(file, number);
                if (!directoryFiles.Any(x => x.Name.ToLower().StartsWith(fileFormatted.ToLower())))
                {
                    missing.Add(fileFormatted);
                }
            }

            return missing;
        }
    }
}