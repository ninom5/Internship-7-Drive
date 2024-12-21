using Drive.Domain.Interfaces;
using Drive.Presentation.Actions;

namespace Drive.Presentation.Menus
{
    public class MainMenu : BaseMenu
    {
        private readonly IUserService _userService;
        private readonly IFolderService _folderService;

        public MainMenu(IUserService userService, IFolderService folderService) : base("Glavni Menu")
        {
            _userService = userService;
            _folderService = folderService;

            Options.Add(("Registracija novog korisnika", new RegisterUserAction(_userService, _folderService)));
            Options.Add(("Prijava korisnika", new LoginAction(_userService, _folderService)));
            Options.Add(("Izlaz iz aplikacije", new ExitAction()));
        }
    }
}
