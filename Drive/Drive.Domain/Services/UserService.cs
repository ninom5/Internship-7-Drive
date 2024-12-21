using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Domain.Interfaces;

namespace Drive.Domain.Services
{
    public class UserService : IUserService 
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public Status Create(string name, string surname, string email, string password, byte[] hashedPassword)
        {
            try
            {
                var newUser = new User
                {
                    Name = name,
                    Surname = surname,
                    Email = email,
                    Password = password,
                    HashedPassword = hashedPassword,
                    CreatedAt = DateTime.UtcNow
                };

                _userRepository.Add(newUser);

                return Status.Success;
            }
            catch (Exception ex)
            {
                return Status.Failed;
            }
        }
        public bool EmailExists(string email)
        {
            return _userRepository.EmailExists(email);
        }

        public bool PasswordsMatch(string email, byte[] password)
        {
            var user = GetUser(email);

            if (user == null)
                return false;

            return user.HashedPassword.SequenceEqual(password);
        }
        public User? GetUser(string email)
        {
            return _userRepository.GetUser(email);
        }
        public IEnumerable<T> GetFoldersOrFiles<T>(User user)
        {
            return _userRepository.GetFoldersOrFiles<T>(user);
        }
    }
}
