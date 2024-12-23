using Drive.Data.Entities.Models;
using Drive.Domain.Enums;

namespace Drive.Domain.Interfaces
{
    public interface IUserService
    {
        bool EmailExists(string email);
        Status Create(string name, string surname, string email, string password, byte[] hashedPassword);
        bool PasswordsMatch(string email, byte[] password);
        User ?GetUser(string email);
        Status UpdateUser(User user);
        IEnumerable<T> GetFoldersOrFiles<T>(User user);
    }
}
