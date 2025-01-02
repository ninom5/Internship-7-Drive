using Drive.Domain.Enums;
using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Reader;
using Drive.Presentation.Utilities;


namespace Drive.Presentation.Actions.Authentication
{
    public class RegisterUserAction : IAction
    {
        private readonly IUserService _userService;
        private readonly IFolderService _folderService;
        public RegisterUserAction(IUserService userService, IFolderService folderService)
        {
            _userService = userService;
            _folderService = folderService;
        }
        public void Execute()
        {
            Console.Clear();

            var user = CollectBaseUserData();

            if (new[] { user.Item1, user.Item2, user.Item3, user.Item4 }.Any(string.IsNullOrEmpty))
                return;

            string captcha = Captcha.GenerateCaptcha();
            Console.WriteLine($"Captcha: {captcha} . Unesite sto pise. Prazno za odustat");

            if (!Captcha.ValidateCaptcha(captcha))
                return;

            byte[] hashedPassword = Hash.HashText(user.Item4);

            Status userCreatingStatus = _userService.Create(user.Item1, user.Item2, user.Item3, user.Item4, hashedPassword);

            if (userCreatingStatus != Status.Success)
            {
                Console.WriteLine("pogreska prilikom registracije");
                return;
            }

            Console.Clear();

            Console.WriteLine("Korisnik uspjesno registriran");

            var createdUser = _userService.GetUser(user.Item3);
            if (createdUser == null) return;

            var rootFolderStatus = _folderService.CreateFolder("Root Folder", createdUser, null);

            if (rootFolderStatus != Status.Success)
            {
                Console.WriteLine("Pogreska prilikom dodavanja root foldera");
                return;
            }

            Console.WriteLine("Root folder uspjesno dodan");

            Program.CurrentUser = null;

            Thread.Sleep(2000);
        }
        private (string, string, string, string) CollectBaseUserData()
        {
            Console.WriteLine("Unesite podatke za registraciju \nZa odustajanje i povratak na prethodni meni unesite praznu liniju\n");

            string? name = ReadInput.ReadString("Unesite ime", input => input.Trim().Split(" ").Length < 2, "Ime ne moze biti dulje od 1 rijeci...");
            if (name == null) return ("", "", "", "");

            string? surname = ReadInput.ReadString("Unesite prezime", input => input.Trim().Split(" ").Length < 2, "Prezime ne moze biti dulje od 1 rijeci...");
            if (surname == null) return ("", "", "", "");

            string? email = Helper.CollectMail(_userService);
            if (string.IsNullOrEmpty(email)) return ("", "", "", "");

            string? password = ReadInput.RegisterPassword("Unesite sifru", input => !string.IsNullOrEmpty(input));
            if (password == null) return ("", "", "", "");

            return (name, surname, email, password);
        }
    }
}
