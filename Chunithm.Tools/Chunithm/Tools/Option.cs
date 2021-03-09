using System.Collections.Generic;
using System.IO;

namespace Chunithm.Tools
{
    // A000, A001 etc..
    public class Option
    {
        public DirectoryInfo DirectoryInfo { get; set; }
        public Dictionary<OptionSubFolderType, List<OptionSubFolder>> OptionSubFolders { get; set; }
    }
}