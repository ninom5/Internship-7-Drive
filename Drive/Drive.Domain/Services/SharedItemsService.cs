using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Domain.Interfaces.Repositories;
using Drive.Domain.Interfaces.Services;
using Drive.Data.Enums;

namespace Drive.Domain.Services
{
    public class SharedItemsService : ISharedItemService
    {
        private readonly ISharedRepository _sharedRepository;

        public SharedItemsService(ISharedRepository sharedRepository)
        {
            _sharedRepository = sharedRepository;
        }
        public Status Create(int itemId, Data.Enums.DataType itemType, User sharedBy, User sharedWith, Folder? folder, Drive.Data.Entities.Models.File? file)
        {
            try
            {
                var newSharedItem = new SharedItem
                {
                    ItemId = itemId,
                    ItemType = itemType,
                    SharedWithId = sharedWith.Id,
                    SharedById = sharedBy.Id,
                    SharedAt = DateTime.UtcNow,
                    SharedWith = sharedWith,
                    SharedBy = sharedBy,
                    Folder = folder,
                    File = file
                };

                _sharedRepository.Add(newSharedItem);

                return Status.Success;
            }
            catch
            {
                return Status.Failed;
            }
        }
        public Status Remove(SharedItem sharedItem)
        {
            _sharedRepository.Delete(sharedItem);

            return Status.Success;
        }
        public SharedItem GetSharedItem(int id, User user, User shareToUser, DataType dataType)
        {
            return _sharedRepository.GetSharedItem(id, user, shareToUser, dataType);
        }
        public IEnumerable<SharedItem> GetAllSharedWithUser(User shareToUser)
        {
            return _sharedRepository.GetAllShared(shareToUser);
        }
        public bool AlreadyShared(int id, int sharedWithId, int sharedById, DataType dataType)
        {
            return _sharedRepository.DoesExist(id, sharedWithId, sharedById, dataType);
        }
    }
}
