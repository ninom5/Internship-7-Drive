using Drive.Data.Entities.Models;

namespace Drive.Domain.Interfaces.Repositories
{
    public interface ISharedRepository
    {
        void Add(SharedItem sharedItem);
    }
}
