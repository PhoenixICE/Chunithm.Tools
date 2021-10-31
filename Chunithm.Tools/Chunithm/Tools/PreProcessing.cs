using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chunithm.Tools
{
    public class PreProcessing
    {
        public Settings Settings { get; set; }
        public Dictionary<FolderType, DirectoryInfo> Folders { get; set; } = new Dictionary<FolderType, DirectoryInfo>();
        public Dictionary<FileType, FileInfo> Files { get; set; } = new Dictionary<FileType, FileInfo>();
        public Dictionary<string, Option> Options { get; set; } = new Dictionary<string, Option>();
        public Dictionary<ChuniAppPatchType, ChunAppPatchStatusType> CurrentChuniAppPatches { get; set; } = new Dictionary<ChuniAppPatchType, ChunAppPatchStatusType>();
        public Dictionary<string, string> OptionsSummary { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, Validator.XMLClasses.EventData> Events { get; set; } = new Dictionary<string, Validator.XMLClasses.EventData>();
        public Hashtable Duplicates { get; set; } = new Hashtable();

        public PreProcessing(Settings settings)
        {
            Settings = settings;
        }

        public async Task Run()
        {
            await Task.Delay(1000);
            Program.LogInfo($"Current Directory: {Directory.GetCurrentDirectory()}");
            GetDefaultFolders();
            GetDefaultFiles();
            GetChuniAppPatches();
            GetAllOptionFiles();
            Program.LogInfo("Completed Pre-Processing.");
            Program.LogWarning("Press Enter to Continue to Menu.");
            Console.ReadLine();
        }

        private void GetChuniAppPatches()
        {
            if (!Files.TryGetValue(FileType.ChuniApp, out var fileInfo))
            {
                Program.LogInfo("Skipping ChuniApp Patches.");
                Program.LogBreak();
                return;
            }

            Program.LogInfo("Checking for ChuniApp Patches...");
            using (var stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.ReadWrite))
            {
                foreach (var chuniAppPatch in Settings.ChuniAppPatches)
                {
                    var checkPatch = new List<ChunAppPatchStatusType>();
                    foreach (var patch in chuniAppPatch.Value)
                    {
                        stream.Position = patch.Offset;
                        var buffer = new byte[patch.On.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        if (Enumerable.SequenceEqual(patch.On, buffer))
                        {
                            checkPatch.Add(ChunAppPatchStatusType.On);
                        }
                        else if (Enumerable.SequenceEqual(patch.Off, buffer))
                        {
                            checkPatch.Add(ChunAppPatchStatusType.Off);
                        }
                        else
                        {
                            checkPatch.Add(ChunAppPatchStatusType.Unknown);
                        }
                    }

                    if (checkPatch.All(x => x == ChunAppPatchStatusType.Off))
                    {
                        CurrentChuniAppPatches[chuniAppPatch.Key] = ChunAppPatchStatusType.Off;
                    }
                    else if (checkPatch.All(x => x == ChunAppPatchStatusType.On))
                    {
                        CurrentChuniAppPatches[chuniAppPatch.Key] = ChunAppPatchStatusType.On;
                    }
                    else
                    {
                        CurrentChuniAppPatches[chuniAppPatch.Key] = ChunAppPatchStatusType.Unknown;
                    }

                    Program.Log($"  Patch: {chuniAppPatch.Key} State: {CurrentChuniAppPatches[chuniAppPatch.Key]}");
                }
            }
            Program.LogInfo("Checking for ChuniApp Patches Completed.");
            Program.LogBreak();
        }

        public ChunAppPatchStatusType FlipChuniAppPatch(ChuniAppPatchType chuniAppPatchType)
        {
            if (!Files.TryGetValue(FileType.ChuniApp, out var fileInfo))
            {
                Program.LogError("Executable not found? Please check your bin Folder.");
                return ChunAppPatchStatusType.Unknown;
            }

            if (!CurrentChuniAppPatches.TryGetValue(chuniAppPatchType, out var chunAppPatchStatusType))
            {
                Program.LogError("Patch not found in Executable?");
                return ChunAppPatchStatusType.Unknown;
            }

            if (chunAppPatchStatusType == ChunAppPatchStatusType.Unknown)
            {
                Program.LogError("Can't Patch this as status is Unknown.");
                return ChunAppPatchStatusType.Unknown;
            }

            if (!Settings.ChuniAppPatches.TryGetValue(chuniAppPatchType, out var chuniAppPatches))
            {
                Program.LogError("Patch not found in Settings file.");
                return ChunAppPatchStatusType.Unknown;
            }

            using (var stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.ReadWrite))
            {
                foreach (var chuniAppPatch in chuniAppPatches)
                {
                    stream.Position = chuniAppPatch.Offset;
                    if (chunAppPatchStatusType == ChunAppPatchStatusType.On)
                    {
                        stream.Write(chuniAppPatch.Off, 0, chuniAppPatch.Off.Length);
                    }
                    else if (chunAppPatchStatusType == ChunAppPatchStatusType.Off)
                    {
                        stream.Write(chuniAppPatch.On, 0, chuniAppPatch.On.Length);
                    }
                }
            }

            if (chunAppPatchStatusType == ChunAppPatchStatusType.On)
            {
                return ChunAppPatchStatusType.Off;
            }
            else if (chunAppPatchStatusType == ChunAppPatchStatusType.Off)
            {
                return ChunAppPatchStatusType.On;
            }
            else
            {
                return ChunAppPatchStatusType.Unknown;
            }
        }

        public bool Restore(OptionSubFolder optionSubFolder, out string log)
        {
            log = null;
            var oldPath = $"{optionSubFolder.XMLFile.FullName}.bak";
            if (!File.Exists(oldPath))
            {
                return false;
            }
            var path = optionSubFolder.XMLFile.FullName;
            File.Delete(optionSubFolder.XMLFile.FullName);

            var oldFile = new FileInfo(oldPath);
            oldFile.MoveTo(path);
            optionSubFolder.XMLFile = new FileInfo(path);
            ProcessXMLFile(optionSubFolder);
            log = $"Restored File {optionSubFolder.XMLFile.FullName}";
            return true;
        }

        public bool Unlock(OptionSubFolder optionSubFolder, out string log)
        {
            log = null;
            if (optionSubFolder.XMLTags.Count == 0 || optionSubFolder.XMLTags.All(x => x.Value))
            {
                //No Tags or already unlocked
                return false;
            }

            foreach (var regex in Settings.RegexMatch)
            {
                optionSubFolder.Contents = Regex.Replace(optionSubFolder.Contents, regex.Key, $"$1{regex.Value}$3", RegexOptions.IgnoreCase);
            }

            var oldPath = optionSubFolder.XMLFile.FullName;
            if (!File.Exists($"{optionSubFolder.XMLFile.FullName}.bak"))
            {
                optionSubFolder.XMLFile.MoveTo($"{optionSubFolder.XMLFile.FullName}.bak");
            }
            File.WriteAllText(oldPath, optionSubFolder.Contents);
            optionSubFolder.XMLFile = new FileInfo(oldPath);
            log = $"Unlocked File {oldPath}";
            foreach (var xmlTag in optionSubFolder.XMLTags.Keys.ToArray())
            {
                optionSubFolder.XMLTags[xmlTag] = !optionSubFolder.XMLTags[xmlTag];
            }
            return true;
        }

        private void GetAllOptionFiles()
        {
            if (!Folders.TryGetValue(FolderType.Options, out var optionsFolder))
            {
                throw new ApplicationException("Missing Options Folder...?");
            }

            if (!Folders.TryGetValue(FolderType.A000, out var A000Folder))
            {
                throw new ApplicationException("Missing A000 Folder...?");
            }

            var optionFolders = optionsFolder.GetDirectories().ToList();
            optionFolders.Add(A000Folder);
            foreach (var optionFolder in optionFolders.OrderBy(x => x.Name))
            {
                Program.LogInfo($"Processing Option Folder: {optionFolder.FullName}");
                ProcessOptionFolder(optionFolder);
            }

            Program.LogInfo("Processing Option Folders Completed.");
            Program.LogBreak();
        }

        private const int ProgressBarSegments = 50;

        private void PrintProcessTrack(int current, int total)
        {
            var percentage = (float)current / total;
            var blocks = (int)Math.Floor(ProgressBarSegments * percentage);
            var progressBar = $"[{new string('=', blocks),-ProgressBarSegments}] {percentage:P}";
            var top = Console.CursorTop;
            Console.SetCursorPosition(0, top);
            Program.Log(progressBar, false);
        }

        private void ProcessOptionFolder(DirectoryInfo optionFolder)
        {
            var option = new Option();
            Options[optionFolder.Name] = option;
            option.DirectoryInfo = optionFolder;
            var optionSubFolders = new Dictionary<OptionSubFolderType, List<OptionSubFolder>>();
            option.OptionSubFolders = optionSubFolders;
            var directories = optionFolder.GetDirectories();

            var dataTable = new DataTable();
            dataTable.Columns.Add("Path", typeof(string));
            dataTable.Columns.Add("Files", typeof(int));
            dataTable.Columns.Add("Unlocked", typeof(string));
            dataTable.Columns.Add("Duplicate", typeof(int));

            for (int i = 0; i < directories.Length; i++)
            {
                DirectoryInfo directory = directories[i];
                PrintProcessTrack(i + 1, directories.Length);
                if (directory.Name == "System Volume Information") continue;

                var isFolderToTrack = Settings.DefaultOptionFolders.TryGetValue(directory.Name, out var optionSubFolderType);

                var optionSubFolder = ProcessoptionTypeFolder(directory.FullName, optionSubFolderType, dataTable);
                if (isFolderToTrack)
                {
                    optionSubFolders[optionSubFolderType] = optionSubFolder;
                }
            }

            if (dataTable.Rows.Count > 0)
            {
                var summary = AsciiTableGenerators.AsciiTableGenerator.CreateAsciiTableFromDataTable(dataTable).ToString();
                OptionsSummary.Add(optionFolder.Name, summary);
                Program.Log(string.Empty);
                Program.Log(summary);
            }
        }

        private object _lock = new object();
        private object _lock2 = new object();

        private List<OptionSubFolder> ProcessoptionTypeFolder(string optionSubFolderPath, OptionSubFolderType optionSubFolderType, DataTable dataTable)
        {
            var optionSubFolders = new List<OptionSubFolder>();
            var directory = new DirectoryInfo(optionSubFolderPath);
            var optionSubFolderDirectories = directory.GetDirectories();

            Parallel.ForEach(optionSubFolderDirectories, (optionSubFolderDirectory) =>
            {
                var optionSubFolder = new OptionSubFolder();
                optionSubFolder.DirectoryInfo = optionSubFolderDirectory;
                optionSubFolder.OptionType = optionSubFolderType;
                var path = Path.Combine(optionSubFolderDirectory.FullName, $"{directory.Name}.xml");
                var xmlFile = new FileInfo(path);
                if (!xmlFile.Exists)
                {
                    Program.LogError($"      Missing XML File from {path}");
                    return;
                }

                optionSubFolder.XMLFile = xmlFile;
                ProcessXMLFile(optionSubFolder);
                optionSubFolder.ParentDirectory = directory;
                lock (_lock)
                {
                    optionSubFolders.Add(optionSubFolder);
                }
            });

            var row = dataTable.NewRow();
            row["Path"] = directory.Name;
            row["Files"] = optionSubFolders.Count();
            row["Unlocked"] = $"{optionSubFolders.Where(x => x.XMLTags.Any()).Count(x => x.XMLTags.Values.All(y => y))}/{optionSubFolders.Where(x => x.XMLTags.Any()).Count()}";
            row["Duplicate"] = optionSubFolders.Count(x => x.IsDuplicate);
            dataTable.Rows.Add(row);

            return optionSubFolders.ToList();
        }

        public void ProcessXMLFile(OptionSubFolder optionSubFolder)
        {
            var content = File.ReadAllText(optionSubFolder.XMLFile.FullName);
            optionSubFolder.Contents = content;
            lock (_lock2)
            {
                if (!Duplicates.ContainsKey(content))
                {
                    Duplicates.Add(content, optionSubFolder.XMLFile.FullName);
                }
                else
                {
                    optionSubFolder.IsDuplicate = true;
                }
            }
            optionSubFolder.XMLTags.Clear();
            foreach (var regex in Settings.RegexMatch)
            {
                var capture = Regex.Match(content, regex.Key, RegexOptions.IgnoreCase);
                if (capture.Success)
                {
                    optionSubFolder.XMLTags.Add(capture.Value, capture.Groups[2].Value.Equals(regex.Value, StringComparison.InvariantCultureIgnoreCase));
                }
            }
        }

        private void GetDefaultFolders()
        {
            Program.LogInfo(@"Finding Folders...");
            foreach (var defaultFolder in Settings.DefaultFolders)
            {
                // Test Folder name in root directory
                if (InsertFolderIfExists(defaultFolder.Value.Name, defaultFolder.Key))
                {
                    continue;
                }

                // Test Parent Folder
                if (defaultFolder.Value.ParentFolder.HasValue && Folders.TryGetValue(defaultFolder.Value.ParentFolder.Value, out var parentFolderPath))
                {
                    var path = Path.Combine(parentFolderPath.FullName, defaultFolder.Value.Name);
                    if (InsertFolderIfExists(path, defaultFolder.Key))
                    {
                        continue;
                    }
                }

                var found = false;
                foreach (var folder in defaultFolder.Value.PotentialPaths)
                {
                    // Test possible paths
                    if (InsertFolderIfExists(folder, defaultFolder.Key))
                    {
                        found = true;
                        continue;
                    }
                }

                if (found) continue;

                // Doesn't exist error out if Mandatory
                if (defaultFolder.Value.Mandatory)
                {
                    throw new ApplicationException($"Can't find {defaultFolder.Key} folder, make sure Chunithm Tools is in the Root Directory.");
                }

                Program.LogWarning($"Cannot locate Folder {defaultFolder.Key}, ignoring...");
            }

            Program.LogInfo(@"Folders Found!");
            Program.LogBreak();
        }

        private bool InsertFolderIfExists(string path, FolderType folderType)
        {
            if (Directory.Exists(path))
            {
                var directory = new DirectoryInfo(path);
                Folders[folderType] = directory;
                Program.Log($"  Folder: {folderType} Path: {directory.FullName}");
                return true;
            }

            return false;
        }

        private void GetDefaultFiles()
        {
            Program.LogInfo(@"Finding Files...");
            foreach (var defaultFile in Settings.DefaultFiles)
            {
                var filePath = Path.Combine(Folders[defaultFile.Value.ParentFolder].FullName, defaultFile.Value.Name);
                if (!InsertFileIfExists(filePath, defaultFile.Key))
                {
                    throw new ApplicationException($"Can't find {defaultFile.Key} file, make sure Chunithm Tools is in the Root Directory.");
                }
            }

            Program.LogInfo(@"Files Found!");
            Program.LogBreak();
        }

        private bool InsertFileIfExists(string path, FileType fileType)
        {
            if (File.Exists(path))
            {
                var file = new FileInfo(path);
                Files[fileType] = file;
                Program.Log($"  File: {fileType} Path: {file.FullName}");
                return true;
            }

            return false;
        }
    }
}