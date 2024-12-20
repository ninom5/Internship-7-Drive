using Drive.Presentation.Actions;
using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Factories;

namespace Drive.Presentation.Menus
{
    public class LoginMenu : BaseMenu
    {
        private readonly IUserService _userService;
        private readonly User _loggedUser;
        public LoginMenu(IUserService userService, User user) : base("LoginMenu")
        {
            _userService = userService;
            _loggedUser = user;

            Console.Title = $"Dobro dosli, {user.Name}";

            Options.Add(("Moj disk", new UserDiskAction(_userService, _loggedUser)));
            Options.Add(("Dijeljeno sa mnom", new UserSharedFilesAction(_userService, _loggedUser)));
            Options.Add(("Postavke Profila", new UserProfileOptionsAction(_userService, _loggedUser)));
            Options.Add(("Odjava iz profila", new LogOutAction(_userService)));
        }
        public void Execute()
        {
            IMenu loginMenu = MenuFactory.CreateLoginMenu(_loggedUser);
            while(true)
            {
                loginMenu.Display();
                loginMenu.HandleInput();
            }
        }
    }
}
