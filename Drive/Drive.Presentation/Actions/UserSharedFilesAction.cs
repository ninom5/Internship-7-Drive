using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces;
using Drive.Presentation.Interfaces;

namespace Drive.Presentation.Actions
{
    public class UserSharedFilesAction : IAction
    {
        private readonly IUserService _userService;
        private readonly User _LoggedUser;

        public UserSharedFilesAction(IUserService userService, User user)
        {
            _userService = userService;
            _LoggedUser = user;
        }

        public void Execute()
        {
            Console.Clear();
        }
    }
}
