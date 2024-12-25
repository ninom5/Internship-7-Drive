using Drive.Data.Entities.Models;
using Drive.Data.Enums;

namespace Drive.Domain.Interfaces.Repositories
{
    public interface ISharedRepository
    {
        void Add(SharedItem sharedItem);
        bool DoesExist(int id, int sharedWithId, int sharedById, DataType dataType);
    }
}
