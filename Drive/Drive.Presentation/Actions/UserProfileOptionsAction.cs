using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Interfaces;

namespace Drive.Presentation.Actions
{
    public class UserProfileOptionsAction : IAction
    {
        private readonly IUserService _userService;
        private readonly User _user;
        public UserProfileOptionsAction(IUserService userService, User user) 
        {
            _userService = userService;
            _user = user;
        }

        public void Execute()
        {
            Console.Clear();
            
        }
    }
}
