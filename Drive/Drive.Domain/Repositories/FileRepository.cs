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
        public void Remove(Drive.Data.Entities.Models.File file)
        {
            _dbContext.Remove(file);
            _dbContext.SaveChanges();
        }
        public Drive.Data.Entities.Models.File ?GetFile(IEnumerable<Drive.Data.Entities.Models.File> userFiles, string name)
        {
            return userFiles.Where(f => f.Name == name).FirstOrDefault();
        }
        public void Update(Drive.Data.Entities.Models.File file)
        {
            _dbContext.Update(file);
            _dbContext.SaveChanges();
        }
    }
}
