

using Drive.Data.Entities.Models;

namespace Drive.Domain.Interfaces
{
    public interface IFolderRepository
    {
        public void AddFolder(Folder folder);
    }
}
