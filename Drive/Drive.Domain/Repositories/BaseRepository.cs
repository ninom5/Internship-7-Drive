using Drive.Data.Entities;

namespace Drive.Domain.Repositories
{
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly DriveDbContext _dbContext;

        protected BaseRepository(DriveDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
