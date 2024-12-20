using Drive.Data.Entities.Models;

namespace Drive.Domain.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        bool EmailExists(string email);
        User? GetUser(string email);
    }
}
