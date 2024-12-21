using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Menus;

namespace Drive.Presentation.Factories
{
    public static class MenuFactory
    {
        private static IUserService _userService;
        private static IFolderService _folderService;
        public static void Initialize(IUserService userService, IFolderService folderService)
        {
            _userService = userService;
            _folderService = folderService;
        }
        public static IMenu CreateMenu(string menuType)
        {
            return menuType switch
            {
                "MainMenu" => new MainMenu(_userService, _folderService),
            };
        }
        public static IMenu CreateLoginMenu(User user)
        {
            return new LoginMenu(_userService, user, _folderService);
        }
    }
}
