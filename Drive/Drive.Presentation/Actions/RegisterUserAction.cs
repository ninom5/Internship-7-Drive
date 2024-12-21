using Drive.Domain.Enums;
using Drive.Domain.Interfaces;
using Drive.Domain.Repositories;
using Drive.Domain.Services;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Reader;
using Drive.Presentation.Utilities;


namespace Drive.Presentation.Actions
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
            Console.WriteLine("Unesite podatke za registraciju \nZa odustajanje i povratak na prethodni meni unesite praznu liniju");

            string ?name = ReadInput.ReadString("Unesite ime", input => !string.IsNullOrEmpty(input), "Ime ne moze biti prazno. Povratak na prethodni meni...");
            if (name == null) return;

            string? surname = ReadInput.ReadString("Unesite prezime", input => !string.IsNullOrEmpty(input), "Prezime ne moze biti prazno. Povratak na prethodni meni...");
            if (surname == null) return;

            string ?email = ReadInput.ReadString("Unesite email: ", input => ReadInput.IsValidEmail(input), "Email mora biti u formati [string min 1 chara]@[string min 2 chara].[string min 3 chara]\n");
            if (email == null) return;
            
            if(_userService.EmailExists(email))
            {
                Console.WriteLine("Uneseni email vec postoji");
                return;
            }

            string ?password = ReadInput.RegisterPassword("Unesite sifru", input => !string.IsNullOrEmpty(input), "Lozinka ne moze biti prazna. Povratak...");
            if (password == null) return;

            string captcha = Captcha.GenerateCaptcha();
            Console.WriteLine($"Captcha: {captcha} . Unesite sto pise. Prazno za odustat");

            if(!Captcha.ValidateCaptcha(captcha))
                return;

            byte[] hashedPassword = Hash.HashText(password);

            Status userCreatingStatus = _userService.Create(name, surname, email, password, hashedPassword);

            if (userCreatingStatus != Status.Success)
            {
                Console.WriteLine("pogreska prilikom registracije");
                return;
            }

            Console.WriteLine("Korisnik uspjesno registriran");

            var user = _userService.GetUser(email);
            if(user == null) return;    

            var rootFolderStatus = _folderService.CreateFolder("Root Folder", user, null, null);

            if(rootFolderStatus != Status.Success)
            {
                Console.WriteLine("Pogreska prilikom dodavanja root foldera");
                return;
            }

            Console.WriteLine("Root folder uspjesno dodan");
        }
    }
}
