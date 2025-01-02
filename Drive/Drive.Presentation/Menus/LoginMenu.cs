using Drive.Presentation.Actions;
using Drive.Data.Entities.Models;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Factories;
using Drive.Presentation.Menus.SubMenu;
using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Actions.Disk;

namespace Drive.Presentation.Menus
{
    public class LoginMenu : BaseMenu
    {
        private readonly IUserService _userService;
        private readonly IFolderService _folderService;
        private readonly IFileService _fileService;
        private readonly ISharedItemService _sharedItemService;
        private readonly ICommentService _commentService;
        private readonly User _loggedUser;
        
        public LoginMenu(IUserService userService, User user, IFolderService folderService, IFileService fileService, ISharedItemService sharedItemService, ICommentService commentService) : base("LoginMenu")
        {

            _userService = userService;
            _loggedUser = user;
            _folderService = folderService;
            _fileService = fileService;
            _sharedItemService = sharedItemService;
            _commentService = commentService;

            Console.Title = $"Dobro dosli, {user.Name}";

            Options.Add(("Moj disk", LoginMenuFactory.CreateAction("Moj disk", _userService, _loggedUser, _folderService, _fileService, _sharedItemService, _commentService)));
            Options.Add(("Dijeljeno sa mnom", LoginMenuFactory.CreateAction("Dijeljeno sa mnom", _userService, _loggedUser, _folderService, _fileService, _sharedItemService, _commentService)));
            Options.Add(("Postavke Profila", LoginMenuFactory.CreateAction("Postavke Profila", _userService, _loggedUser, _folderService, _fileService, _sharedItemService, _commentService)));
            Options.Add(("Odjava iz profila", LoginMenuFactory.CreateAction("Odjava iz profila", _userService, _loggedUser, _folderService, _fileService, _sharedItemService, _commentService)));
        }
        public void Execute()
        {

            IMenu loginMenu = Program.CurrentUser == null ? MenuFactory.CreateMenu("MainMenu", null) : MenuFactory.CreateMenu("LoginMenu", Program.CurrentUser);
            while(true)
            {
                loginMenu.Display();
                loginMenu.HandleInput();
            }
        }
    }
}
