using Drive.Data.Entities.Models;
using Drive.Domain.Enums;


namespace Drive.Domain.Interfaces.Services
{
    public interface ISharedItemService
    {
        Status Create(int itemId, Data.Enums.DataType itemType, User sharedBy, User sharedWith);
    }
}
