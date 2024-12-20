using Drive.Domain.Interfaces;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Reader;


namespace Drive.Presentation.Actions
{
    public class LoginAction : IAction
    {
        private readonly IUserService _userService;
        public LoginAction(IUserService userService)
        {
            _userService = userService;
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
            }

            int i = 3;
            while(!ReadInput.CheckUserPassword(userEmail, _userService))
            {
                Console.WriteLine($"Unesena sifra nije ispravna imate jos {i--} pokusaja");
                if(i == -1)
                {
                    Console.WriteLine("Povratak na glavni menu...");
                    return;
                }
            }

            Console.WriteLine("Uspjesno ste prijavljeni\n Preusmjeravanje na vaš izbornik...");
            Thread.Sleep(1000);
        }
    }
}
