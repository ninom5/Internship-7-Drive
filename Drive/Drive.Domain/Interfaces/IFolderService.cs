

using Drive.Data.Entities.Models;
using Drive.Domain.Enums;

namespace Drive.Domain.Interfaces
{
    public interface IFolderService
    {
        public Status CreateFolder(string folderName, User user, int? currenteFolderId, Folder? parentFolder);
    }
}
