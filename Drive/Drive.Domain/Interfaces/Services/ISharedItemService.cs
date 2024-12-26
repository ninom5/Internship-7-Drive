using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Data.Enums;

namespace Drive.Domain.Interfaces.Services
{
    public interface ISharedItemService
    {
        Status Create(int itemId, Data.Enums.DataType itemType, User sharedBy, User sharedWith);
        bool AlreadyShared(int id, int sharedWithId, int sharedById, DataType dataType);
        Status Remove(SharedItem sharedItem);
        SharedItem GetSharedItem(int id, User user, User shareToUser, DataType dataType);
    }
}
