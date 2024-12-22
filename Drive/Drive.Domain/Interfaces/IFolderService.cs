using Drive.Data.Entities.Models;
using Drive.Domain.Enums;

namespace Drive.Domain.Interfaces
{
    public interface IFolderService
    {
        public Status CreateFolder(string folderName, User user, Folder? parentFolder);
        public Status DeleteFolder(Folder folder);
    }
}
