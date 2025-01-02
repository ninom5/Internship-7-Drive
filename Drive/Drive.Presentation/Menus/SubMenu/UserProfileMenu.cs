using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Actions.Profile;
using Drive.Presentation.Interfaces;

namespace Drive.Presentation.Menus.SubMenu
{
    public class UserProfileMenu : BaseMenu, IAction
    {
        private readonly IUserService _userService;
        private readonly User _user;
        public UserProfileMenu(IUserService userService, User user) : base("Postavke profila")
        {
            _userService = userService;
            _user = user;
            Options.Add(("Promjena emaila", new ChangeUserEmailAction(_userService, _user)));
            Options.Add(("Promjena lozinke", new ChangeUserPasswordAction(_userService, _user)));
        }
        public void Execute()
        {
            Display();
            HandleInput();
        }
    }
}
