using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Interfaces;

namespace Drive.Presentation.Actions
{
    public class UserSharedFilesAction : IAction
    {
        private readonly IUserService _userService;
        private readonly ISharedItemService _sharedItemService;
        private readonly User _LoggedUser;

        public UserSharedFilesAction(IUserService userService, ISharedItemService sharedItemService, User user)
        {
            _userService = userService;
            _sharedItemService = sharedItemService;
            _LoggedUser = user;
        }

        public void Execute()
        {
            Console.Clear();
        }
    }
}
