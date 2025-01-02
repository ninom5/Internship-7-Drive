using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Reader;
using Drive.Presentation.Utilities;

namespace Drive.Presentation.Actions.Profile
{
    public class ChangeUserPasswordAction : IAction
    {
        private readonly IUserService _userService;
        private readonly User user;

        public ChangeUserPasswordAction(IUserService userService, User user)
        {
            _userService = userService;
            this.user = user;
        }

        public void Execute()
        {
            Console.WriteLine("Unesite novu sifru: ");

            var newPassword = ReadInput.RegisterPassword("Unesite sifru", input => !string.IsNullOrEmpty(input));
            if (string.IsNullOrEmpty(newPassword))
            {
                Console.WriteLine("Lozinka ne moze biti prazna. Povratak...");
                return;
            }

            string captcha = Captcha.GenerateCaptcha();
            Console.WriteLine($"Captcha: {captcha} . Unesite sto pise. Prazno za odustat");

            if (!Captcha.ValidateCaptcha(captcha))
                return;

            byte[] hashedPassword = Hash.HashText(newPassword);

            user.Password = newPassword;
            user.HashedPassword = hashedPassword;

            var status = _userService.UpdateUser(user);

            if (status != Status.Success)
            {
                Console.WriteLine("Pogreska prilikom updateanja lozinke");
                return;
            }

            Console.WriteLine($"Lozinka uspjesno promijenjena u: {user.Password}");
        }
    }
}
