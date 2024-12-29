using Drive.Domain.Interfaces.Services;
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
        private readonly ISharedItemService _sharedItemService;
        private readonly ICommentService _commentService;

        private static int failedPasswordCountdown = 30;
        private static Timer timer;
        public LoginAction(IUserService userService, IFolderService folderService, IFileService fileService, ISharedItemService sharedItemService, ICommentService commentService)
        {
            _userService = userService;
            _folderService = folderService;
            _fileService = fileService;
            _sharedItemService = sharedItemService;
            _commentService = commentService;
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

            if(!ReadInput.CheckUserPassword(userEmail, _userService))
            {
                Console.WriteLine($"Unesena sifra nije ispravna. Povratak na glavni izbornik...");

                StartCountdown();

                while (failedPasswordCountdown > 0)
                {
                    Thread.Sleep(1000);
                }

                Console.WriteLine("Povratak na glavni menu...");

                return;
            }

            Console.WriteLine("\nUspjesno ste prijavljeni\n Preusmjeravanje na vaš izbornik...");

            var user = _userService.GetUser(userEmail);
            if(user == null)
            {
                Console.WriteLine("Pogreska prilikom logiranja");
                return;
            }

            var loginMenu = new LoginMenu(_userService, user, _folderService, _fileService, _sharedItemService, _commentService);
            loginMenu.Execute();
        }

        private static void StartCountdown()
        {
            timer = new Timer(OnTimerElapsed, null, 0, 1000);
        }
        private static void OnTimerElapsed(object state)
        {
            if (failedPasswordCountdown > 0) 
            {
                failedPasswordCountdown--;
                Console.Clear();
                Console.WriteLine($"Unesena sifra nije ispravna. Povratak na glavni izbornik za: {failedPasswordCountdown}");
            }
        }
    }
}
