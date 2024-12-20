using Drive.Data.Entities;
using Drive.Data.Entities.Models;

namespace Drive.Domain.Repositories
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(DriveDbContext dbContext) : base(dbContext) 
        {
        }
        public (List<Folder>, List<Drive.Data.Entities.Models.File>) GetUserFiles(User user)
        {
            var folders = user.Folders.ToList();
            var files = user.Files.ToList();

            return (folders, files);
        }
    }
}
