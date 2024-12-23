using Drive.Data.Entities.Models;

namespace Drive.Domain.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        void Update(User user);
        bool EmailExists(string email);
        User? GetUser(string email);
        IEnumerable<T> GetFoldersOrFiles<T>(User user);
    }
}
