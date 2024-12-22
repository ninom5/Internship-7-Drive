using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Menus;
using System.Runtime.CompilerServices;

namespace Drive.Presentation.Factories
{
    public static class MenuFactory
    {
        private static IUserService _userService;
        private static IFolderService _folderService;
        private static IFileService _fileService;
        public static void Initialize(IUserService userService, IFolderService folderService, IFileService fileService)
        {
            _userService = userService;
            _folderService = folderService;
            _fileService = fileService;
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
            return new LoginMenu(_userService, user, _folderService, _fileService);
        }
    }
}
