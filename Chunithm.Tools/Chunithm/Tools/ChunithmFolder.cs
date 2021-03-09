namespace Chunithm.Tools
{
    public struct ChunithmFolder
    {
        public ChunithmFolder(string name, FolderType? parentFolder, string[] potentialPaths, bool mandatory = true)
        {
            Name = name;
            ParentFolder = parentFolder;
            PotentialPaths = potentialPaths;
            Mandatory = mandatory;
        }

        public string Name { get; set; }
        public string[] PotentialPaths { get; set; }
        public FolderType? ParentFolder { get; set; }
        public bool Mandatory { get; set; }
    }
}