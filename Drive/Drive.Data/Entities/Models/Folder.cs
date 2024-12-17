using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.Data.Entities.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public int? ParentFolderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public User Owner { get; set; }
        public Folder ParentFolder { get; set; }
        public ICollection<Folder> Subfolders { get; set; }
        public ICollection<File> Files { get; set; }
    }
}
