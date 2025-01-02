using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Actions;
using Drive.Presentation.Actions.Authentication;
using Drive.Presentation.Factories;

namespace Drive.Presentation.Menus
{
    public class MainMenu : BaseMenu
    {
        private readonly IUserService _userService;
        private readonly IFolderService _folderService;
        private readonly IFileService _fileService;
        private readonly ISharedItemService _sharedItemService;
        private readonly ICommentService _commentService;
        public MainMenu(IUserService userService, IFolderService folderService, IFileService fileService, ISharedItemService sharedItemService, ICommentService commentService) : base("Glavni Menu")
        {
            Console.Title = "DUMP Drive";

            _userService = userService;
            _folderService = folderService;
            _fileService = fileService;
            _sharedItemService = sharedItemService;
            _commentService = commentService;

            Options.Add(("Registracija novog korisnika", AuthenticationFactory.CreateAction("RegisterUserAction", userService, folderService, fileService, sharedItemService, commentService)));
            Options.Add(("Prijava korisnika", AuthenticationFactory.CreateAction("LoginAction", _userService, _folderService, _fileService, _sharedItemService, _commentService)));
            Options.Add(("Izlaz iz aplikacije", new ExitAction()));
        }
    }

}
