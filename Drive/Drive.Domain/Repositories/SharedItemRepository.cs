using Drive.Data.Entities;
using Drive.Data.Entities.Models;
using Drive.Data.Enums;
using Drive.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;


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
        public bool DoesExist(int id, int sharedWithId, int sharedById, Data.Enums.DataType dataType)
        {
            return _dbContext.SharedItems.Any(item => item.ItemId == id && item.SharedWithId == sharedWithId && item.ItemType == dataType && item.SharedById == sharedById);
        }
        public void Delete(SharedItem sharedItem)
        {
            _dbContext.Remove(sharedItem);
            _dbContext.SaveChanges();
        }
        public SharedItem GetSharedItem(int id, User user, User shareToUser, Drive.Data.Enums.DataType dataType)
        {
            return _dbContext.SharedItems.FirstOrDefault(item => item.ItemId == id && item.SharedWithId == shareToUser.Id && item.ItemType == dataType && item.SharedById == user.Id);
        }
        public IEnumerable<SharedItem> GetAllShared(User shareToUser, DataType dataType)
        {
            return _dbContext.SharedItems
                .Where(item => item.SharedWithId == shareToUser.Id && item.ItemType == dataType)
                .Include(item => item.Folder) 
                .Include(item => item.File) 
                .Include(item => item.Folder.Owner)
                .Include(item => item.File.Owner)
                .Include(item => item.File.Folder)
                .AsNoTracking()
                .ToList();
        }
    }
}
