using Drive.Data.Entities.Models;

namespace Drive.Domain.Interfaces.Repositories
{
    public interface ISharedRepository
    {
        void Add(SharedItem sharedItem);
        bool DoesExist(int id, int sharedWithId, int sharedById, Drive.Data.Enums.DataType dataType);
        void Delete(SharedItem sharedItem);
        SharedItem GetSharedItem(int id, User user, User shareToUser, Drive.Data.Enums.DataType dataType);
        IEnumerable<SharedItem> GetAllShared(User shareToUser, Drive.Data.Enums.DataType dataType);
        public IEnumerable<SharedItem> GetAllUserShared(User user);
    }
}
