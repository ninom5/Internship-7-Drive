using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Data.Enums;

namespace Drive.Domain.Interfaces.Services
{
    public interface ISharedItemService
    {
        Status Create(int itemId, Data.Enums.DataType itemType, User sharedBy, User sharedWith, Folder? folder, Drive.Data.Entities.Models.File? file);
        bool AlreadyShared(int id, int sharedWithId, int sharedById, DataType dataType);
        Status Remove(SharedItem sharedItem);
        SharedItem GetSharedItem(int id, User user, User shareToUser, DataType dataType);
        IEnumerable<SharedItem> GetAllSharedWithUser(User shareToUser, DataType dataType);
    }
}
