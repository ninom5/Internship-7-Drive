using Drive.Data.Entities;
using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Repositories;

namespace Drive.Domain.Repositories
{
    public class SharedItemRepository : BaseRepository<SharedItem>,  ISharedRepository
    {
        public SharedItemRepository(DriveDbContext dbContext) : base(dbContext)
        {
        }
        public void Add(SharedItem sharedItem)
        {
            _dbContext.Add(sharedItem);
            _dbContext.SaveChanges();
        }
    }
}
