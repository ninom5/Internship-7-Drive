namespace Drive.Data.Entities.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }

        public File File { get; set; }
        public User User { get; set; }
    }
}
