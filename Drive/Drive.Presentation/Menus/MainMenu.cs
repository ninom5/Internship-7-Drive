using Drive.Presentation.Actions;

namespace Drive.Presentation.Menus
{
    public class MainMenu : BaseMenu
    {
        public MainMenu() : base("Glavni Menu")
        {
            Options.Add(("Registracija novog korisnika", new RegisterUserAction()));
            Options.Add(("Prijava korisnika", new LoginAction()));
            Options.Add(("Izlaz iz aplikacije", new ExitAction()));
        }
    }
}
