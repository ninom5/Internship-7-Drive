using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces;
using Drive.Domain.Services;
using Drive.Presentation.Actions;
using Drive.Presentation.Interfaces;
using System.Security.Cryptography.X509Certificates;

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
            //Options.Add("Promjena lozinke", new);
        }
        public void Execute()
        {
            Display();
            HandleInput();
        }
    }
}
