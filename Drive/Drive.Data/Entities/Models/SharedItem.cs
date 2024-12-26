namespace Drive.Data.Entities.Models
{
    public class SharedItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Data.Enums.DataType ItemType { get; set; }
        public int SharedWithId { get; set; }
        public int SharedById { get; set; }
        public DateTime SharedAt { get; set; }
        public Folder? Folder { get; set; }
        public Drive.Data.Entities.Models.File? File { get; set; }
        public User SharedWith { get; set; } = null!;
        public User SharedBy { get; set; } = null!;
    }
}