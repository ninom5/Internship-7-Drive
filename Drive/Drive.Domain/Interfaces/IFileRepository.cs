

namespace Drive.Domain.Interfaces
{
    public interface IFileRepository
    {
        public void AddFile(Drive.Data.Entities.Models.File file);
        public void Remove(Drive.Data.Entities.Models.File file);
    }
}
