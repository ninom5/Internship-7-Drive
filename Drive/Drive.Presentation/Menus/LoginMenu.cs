using Drive.Presentation.Actions;
using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Factories;
using Drive.Presentation.Menus.SubMenu;

namespace Drive.Presentation.Menus
{
    public class LoginMenu : BaseMenu
    {
        private readonly IUserService _userService;
        private readonly IFolderService _folderService;
        private readonly IFileService _fileService;
        private readonly User _loggedUser;
        public LoginMenu(IUserService userService, User user, IFolderService folderService, IFileService fileService) : base("LoginMenu")
        {
            _userService = userService;
            _loggedUser = user;
            _folderService = folderService;
            _fileService = fileService;

            Console.Title = $"Dobro dosli, {user.Name}";

            Options.Add(("Moj disk", new UserDiskAction(_userService, _folderService, _loggedUser, _fileService)));
            Options.Add(("Dijeljeno sa mnom", new UserSharedFilesAction(_userService, _loggedUser)));
            Options.Add(("Postavke Profila", new UserProfileMenu(_userService, _loggedUser)));
            Options.Add(("Odjava iz profila", new LogOutAction(_userService, _folderService)));
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
