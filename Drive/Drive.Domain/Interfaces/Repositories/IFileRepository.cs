namespace Drive.Domain.Interfaces.Repositories
{
    public interface IFileRepository
    {
        public void AddFile(Data.Entities.Models.File file);
        public void Remove(Data.Entities.Models.File file);
        public void Update(Data.Entities.Models.File file);
    }
}
