using Drive.Data.Entities;
using Drive.Domain.Interfaces;


namespace Drive.Domain.Repositories
{
    public class FileRepository : BaseRepository<Drive.Data.Entities.Models.File>, IFileRepository
    {
        public FileRepository(DriveDbContext dbContext) : base(dbContext)
        {
        }

        public void AddFile(Drive.Data.Entities.Models.File file)
        {
            _dbContext.Add(file);
            _dbContext.SaveChanges();
        }
    }
}
