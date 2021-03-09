namespace Chunithm.Tools
{
    public struct ChunithmFile
    {
        public ChunithmFile(string name, FileType file, FolderType parentFolder, string[] potentialPaths, bool mandatory = true)
        {
            Name = name;
            File = file;
            PotentialPaths = potentialPaths;
            ParentFolder = parentFolder;
            Mandatory = mandatory;
        }

        public FolderType ParentFolder { get; set; }
        public string Name { get; set; }
        public string[] PotentialPaths { get; set; }
        public FileType File { get; set; }
        public bool Mandatory { get; set; }
    }
}