using System.Collections.Generic;
using System.IO;

namespace Chunithm.Tools
{
    // chara, course etc..
    public class OptionSubFolder
    {
        public DirectoryInfo DirectoryInfo { get; set; }
        public OptionSubFolderType OptionType { get; set; }
        public FileInfo XMLFile { get; set; }
        public string Contents { get; set; }
        public Dictionary<string, bool> XMLTags { get; set; } = new Dictionary<string, bool>();
        public bool IsDuplicate { get; set; }
        public DirectoryInfo ParentDirectory { get; set; }
    }
}