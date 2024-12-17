namespace Drive.Data.Entities.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int OwnerId { get; set; } 
        public int? ParentFolderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public User Owner { get; set; } = null!;
        public Folder ParentFolder { get; set; } = null!;
        public ICollection<Folder> Subfolders { get; set; } = null!;
        public ICollection<File> Files { get; set; } = null!;
    }
}
