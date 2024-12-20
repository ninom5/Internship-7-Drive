using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces;
using Drive.Presentation.Interfaces;

namespace Drive.Presentation.Actions
{
    public class UserDiskAction : IAction
    {
        private readonly IUserService _userService;
        private readonly User _user;

        public UserDiskAction(IUserService userService, User user)
        {
            _userService = userService;
            _user = user;
        }

        public void Execute()
        {
            Console.Clear();
        }
    }
}
