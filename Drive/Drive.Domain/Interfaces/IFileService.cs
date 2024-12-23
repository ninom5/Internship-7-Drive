using Drive.Data.Entities.Models;
using Drive.Domain.Enums;


namespace Drive.Domain.Interfaces
{
    public interface IFileService
    {
        public Status CreateFile(string fileName, string content, User user, Folder folder);
        public Status DeleteFile(Drive.Data.Entities.Models.File file);
        public Status UpdateFile(Drive.Data.Entities.Models.File file, string newName);
        public Status UpdateFileContent(Drive.Data.Entities.Models.File file);
    }
}
