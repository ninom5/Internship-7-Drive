using Drive.Data.Entities.Models;

namespace Drive.Domain.Interfaces.Repositories
{
    public interface IFolderRepository
    {
        public void AddFolder(Folder folder);
        public void RemoveFolder(Folder folder);
        public void UpdateFolder(Folder folder, string name);
    }
}
