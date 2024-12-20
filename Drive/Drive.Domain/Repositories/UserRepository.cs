using Drive.Data.Entities;
using Drive.Data.Entities.Models;

namespace Drive.Domain.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(DriveDbContext dbContext) : base(dbContext) 
        {
        }
        
        public void Add(User user)
        {
            _dbContext.Users.Add(user); 
            _dbContext.SaveChanges();
        }
    }
}
