using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Domain.Interfaces;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Utilities;

namespace Drive.Presentation.Actions
{
    public class ChangeUserEmailAction : IAction
    {
        private readonly IUserService _userService;
        private readonly User user;
        public ChangeUserEmailAction(IUserService userService, User user)
        {
            _userService = userService;
            this.user = user;
        }

        public void Execute()
        {
            var newEmail = Helper.CollectMail(_userService);

            if (string.IsNullOrEmpty(newEmail))
                return;

            user.Email = newEmail;
            var status = _userService.UpdateUser(user);

            if (status != Status.Success)
            {
                Console.WriteLine("pogreska prilikom azuriranja maila");
                return;
            }

            Console.WriteLine($"Uspjesno azuriran mail u: {user.Email}");
        }
    }
}
