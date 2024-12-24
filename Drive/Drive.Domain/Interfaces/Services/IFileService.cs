using Drive.Data.Entities.Models;
using Drive.Domain.Enums;


namespace Drive.Domain.Interfaces.Services
{
    public interface IFileService
    {
        public Status CreateFile(string fileName, string content, User user, Folder folder);
        public Status DeleteFile(Data.Entities.Models.File file);
        public Status UpdateFile(Data.Entities.Models.File file, string newName);
        public Status UpdateFileContent(Data.Entities.Models.File file);
    }
}
