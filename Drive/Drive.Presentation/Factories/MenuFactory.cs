using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Menus;

namespace Drive.Presentation.Factories
{
    public static class MenuFactory
    {
        private static IUserService _userService = null!;
        private static IFolderService _folderService = null!;
        private static IFileService _fileService = null!;
        private static ISharedItemService _sharedItemService = null!;
        public static void Initialize(IUserService userService, IFolderService folderService, IFileService fileService, ISharedItemService sharedItemService)
        {
            _userService = userService;
            _folderService = folderService;
            _fileService = fileService;
            _sharedItemService = sharedItemService;
        }
        public static IMenu CreateMenu(string menuType)
        {
            return menuType switch
            {
                "MainMenu" => new MainMenu(_userService, _folderService, _fileService, _sharedItemService),
            };
        }
        public static IMenu CreateLoginMenu(User user)
        {
            return new LoginMenu(_userService, user, _folderService, _fileService, _sharedItemService);
        }
    }
}
