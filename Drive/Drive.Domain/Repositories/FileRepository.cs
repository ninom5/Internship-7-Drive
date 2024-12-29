using Drive.Data.Entities;
using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Repositories;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Domain.Repositories
{
    public class FileRepository : BaseRepository<File>, IFileRepository
    {
        public FileRepository(DriveDbContext dbContext) : base(dbContext)
        {
        }

        public void AddFile(File file)
        {
            _dbContext.Add(file);
            _dbContext.SaveChanges();
        }
        public void Remove(File file)
        {
            _dbContext.Remove(file);
            _dbContext.SaveChanges();
        }
        public static File ?GetFile(IEnumerable<File> userFiles, string name)
        {
            return userFiles.Where(f => f.Name == name).FirstOrDefault();
        }
        public void Update(File file)
        {
            _dbContext.Update(file);
            _dbContext.SaveChanges();
        }
        public File GetFileByName(string name, User user)
        {
            return _dbContext.Files.FirstOrDefault(item => item.Name == name && item.Owner == user);
        }
    }
}
