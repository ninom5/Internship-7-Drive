using Drive.Data.Entities.Models;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Domain.Interfaces.Repositories
{
    public interface IFileRepository
    {
        public void AddFile(File file);
        public void Remove(File file);
        public void Update(File file);
        public File GetFileByName(string name, User user);
    }
}
