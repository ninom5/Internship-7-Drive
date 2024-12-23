using Drive.Data.Entities;
using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces;

namespace Drive.Domain.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DriveDbContext dbContext) : base(dbContext)
        {
        }

        public void Add(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }
        public void Update(User user)
        {
            _dbContext.Update(user);
            _dbContext.SaveChanges();
        }
        public bool EmailExists(string email)
        {
            return _dbContext.Users.Any(u => u.Email == email);
        }
        public IEnumerable<T> GetFoldersOrFiles<T>(User user)
        {
            if(typeof(T) == typeof(Folder))
            {
                return _dbContext.Folders.Where(u => u.OwnerId == user.Id).Cast<T>().ToList();
            }
            
            return _dbContext.Files.Where(f => f.OwnerId ==  user.Id).Cast<T>().ToList();
        }
        public User? GetUser(string email)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
