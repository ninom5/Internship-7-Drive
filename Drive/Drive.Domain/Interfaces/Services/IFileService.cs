using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Domain.Interfaces.Services
{
    public interface IFileService
    {
        public Status CreateFile(string fileName, string content, User user, Folder folder);
        public Status DeleteFile(File file);
        public Status UpdateFile(File file, string newName);
        public Status UpdateFileContent(File file);
        public File GetFileByName(string name, User user);
    }
}
