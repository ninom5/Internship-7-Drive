using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Actions;

namespace Drive.Presentation.Menus
{
    public class MainMenu : BaseMenu
    {
        private readonly IUserService _userService;
        private readonly IFolderService _folderService;
        private readonly IFileService _fileService;
        private readonly ISharedItemService _sharedItemService;
        public MainMenu(IUserService userService, IFolderService folderService, IFileService fileService, ISharedItemService sharedItemService) : base("Glavni Menu")
        {
            _userService = userService;
            _folderService = folderService;
            _fileService = fileService;
            _sharedItemService = sharedItemService;

            Options.Add(("Registracija novog korisnika", new RegisterUserAction(_userService, _folderService)));
            Options.Add(("Prijava korisnika", new LoginAction(_userService, _folderService, _fileService, _sharedItemService)));
            Options.Add(("Izlaz iz aplikacije", new ExitAction()));
        }
    }
}
