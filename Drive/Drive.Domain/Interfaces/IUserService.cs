
using Drive.Domain.Enums;

namespace Drive.Domain.Interfaces
{
    public interface IUserService
    {
        bool EmailExists(string email);
        Status Create(string name, string surname, string email, string password, byte[] hashedPassword);
    }
}
