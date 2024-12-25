﻿using Drive.Data.Entities;
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
        public bool DoesExist(int id, int sharedWithId, int sharedById, Data.Enums.DataType dataType)
        {
            return _dbContext.SharedItems.Any(item => item.ItemId == id && item.SharedWithId == sharedWithId && item.ItemType == dataType && item.SharedById == sharedById);
        }
    }
}
