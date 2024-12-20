namespace Drive.Data.Entities.Models
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int FolderId {  get; set; }
        public int OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public Folder Folder { get; set; } = null!;
        public User Owner { get; set; } = null!;
    }
}
