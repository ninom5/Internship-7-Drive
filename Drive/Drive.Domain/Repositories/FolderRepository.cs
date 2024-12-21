
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
    }
}
