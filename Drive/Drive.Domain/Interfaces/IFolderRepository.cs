

using Drive.Data.Entities.Models;

namespace Drive.Domain.Interfaces
{
    public interface IFolderRepository
    {
        public void AddFolder(Folder folder);
        public void RemoveFolder(Folder folder);
    }
}
