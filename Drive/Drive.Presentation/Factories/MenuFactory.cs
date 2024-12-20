using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Menus;

namespace Drive.Presentation.Factories
{
    public static class MenuFactory
    {
        private static IUserService _userService;
        public static void Initialize(IUserService userService)
        {
            _userService = userService;
        }
        public static IMenu CreateMenu(string menuType)
        {
            return menuType switch
            {
                "MainMenu" => new MainMenu(_userService),
            };
        }
        public static IMenu CreateLoginMenu(User user)
        {
            return new LoginMenu(_userService, user);
        }
    }
}
