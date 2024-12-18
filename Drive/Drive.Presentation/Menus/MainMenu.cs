using Drive.Domain.Interfaces;
using Drive.Presentation.Actions;

namespace Drive.Presentation.Menus
{
    public class MainMenu() : BaseMenu
    {
        private readonly IUserService _userService;
        public MainMenu(IUserService userService)
        {
            _userService = userService;
        }
        public MainMenu() : base("Glavni Menu")
        {
            
            Options.Add(("Registracija novog korisnika", new RegisterUserAction(_userService)));
            Options.Add(("Prijava korisnika", new LoginAction()));
            Options.Add(("Izlaz iz aplikacije", new ExitAction()));
        }
    }
}
