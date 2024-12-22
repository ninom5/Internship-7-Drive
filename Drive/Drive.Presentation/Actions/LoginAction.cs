using Drive.Domain.Interfaces;
using Drive.Domain.Services;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Menus;
using Drive.Presentation.Reader;


namespace Drive.Presentation.Actions
{
    public class LoginAction : IAction
    {
        private readonly IUserService _userService;
        private readonly IFolderService _folderService;
        private readonly IFileService _fileService;
        public LoginAction(IUserService userService, IFolderService folderService, IFileService fileService)
        {
            _userService = userService;
            _folderService = folderService;
            _fileService = fileService;
        }
        public void Execute()
        {
            Console.Clear();
            Console.WriteLine("Unesite podatke za prijavu \nUnesite vas email s kojim ste se registrirali");
            var userEmail = Console.ReadLine();

            if (string.IsNullOrEmpty(userEmail))
            {
                Console.WriteLine("Email ne moze biti prazan. \nPovratak na glavni menu...");
                return;
            }

            if(!_userService.EmailExists(userEmail))
            {
                Console.WriteLine("Korisnik s unesenim mailom nije registriran"); 
                return;
            }

            int i = 3;
            if(!ReadInput.CheckUserPassword(userEmail, _userService))
            {
                Console.WriteLine($"Unesena sifra nije ispravna. Povratak na glavni izbornik...");
                Thread.Sleep(30000);
                return;
            }

            Console.WriteLine("Uspjesno ste prijavljeni\n Preusmjeravanje na vaš izbornik...");

            var user = _userService.GetUser(userEmail);
            if(user == null)
            {
                Console.WriteLine("Pogreska");
                return;
            }

            var loginMenu = new LoginMenu(_userService, user, _folderService, _fileService);
            loginMenu.Execute();
        }
    }
}
