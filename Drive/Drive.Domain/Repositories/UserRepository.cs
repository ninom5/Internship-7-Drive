using Drive.Data.Entities;

namespace Drive.Domain.Repositories
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(DriveDbContext dbContext) : base(dbContext) 
        {
        }
    }
}
