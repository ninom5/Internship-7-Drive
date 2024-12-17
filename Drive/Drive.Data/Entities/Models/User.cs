namespace Drive.Data.Entities.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
        public byte[] HashedPassword { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Folder> Folders { get; set; }
        public ICollection<File> Files { get; set; }
        public ICollection<SharedItem> SharedItems { get; set; }
    }
}
