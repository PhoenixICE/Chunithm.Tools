using System.Collections.Generic;

namespace Chunithm.Tools.Interface
{
    public interface IFolderValidator
    {
        IEnumerable<string> ValidateFolder(IEnumerable<OptionSubFolder> optionSubFolders);
    }
}