using Drive.Presentation.Actions;
using Drive.Data.Entities.Models;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Factories;
using Drive.Presentation.Menus.SubMenu;
using Drive.Domain.Interfaces.Services;

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

            Options.Add(("Moj disk", new UserDiskAction(_userService, _folderService, _loggedUser, _fileService, _sharedItemService, _commentService)));
            Options.Add(("Dijeljeno sa mnom", new UserSharedFilesAction(_userService, _loggedUser, _sharedItemService, _fileService, _commentService)));
            Options.Add(("Postavke Profila", new UserProfileMenu(_userService, _loggedUser)));
            Options.Add(("Odjava iz profila", new LogOutAction(_userService, _folderService)));
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
