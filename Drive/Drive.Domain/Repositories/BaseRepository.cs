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

        //public Status Add(T entity)
        //{
        //    _dbContext.Set<T>().Add(entity);
        //    _dbContext.SaveChanges();

        //    return Status.Success;
        //}
    }
}
