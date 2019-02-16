namespace Application.Files.Models
{
    public class DirectoryInfoDto
    {
        public DirectoryFolderItemModel[] Directories { get; set; }

        public DirectoryFileItemModel[] Files { get; set; }
    }

    public class DirectoryFolderItemModel
    {
        public DirectoryFolderItemModel(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    public class DirectoryFileItemModel
    {
        public DirectoryFileItemModel(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
