
using Drive.Data.Entities;
using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces;

namespace Drive.Domain.Repositories
{
    public class FolderRepository : BaseRepository<Folder>, IFolderRepository
    {
        public FolderRepository(DriveDbContext dbContext) : base(dbContext)
        {
        }

        public void AddFolder(Folder folder)
        {
            _dbContext.Folders.Add(folder);
            _dbContext.SaveChanges();
        }
        public static Folder? GetFolder(IEnumerable<Folder> userFolders)
        {
            return userFolders.FirstOrDefault(f => f.Name == "Root Folder");
        }
        public static Folder? GetFolder(IEnumerable<Folder> userFolders, string name)
        {
            return userFolders.FirstOrDefault(f => f.Name == name);
        }
    }
}
