using Drive.Data.Entities.Models;
using Drive.Domain.Enums;

namespace Drive.Domain.Interfaces.Services
{
    public interface IFolderService
    {
        public Status CreateFolder(string folderName, User user, Folder? parentFolder);
        public Status DeleteFolder(Folder folder);
        public Status UpdateFolder(Folder folder, string name);
    }
}
