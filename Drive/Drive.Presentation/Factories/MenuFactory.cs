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
        private static ICommentService _commentService = null!;
        public static void Initialize(IUserService userService, IFolderService folderService, IFileService fileService, ISharedItemService sharedItemService, ICommentService commentService)
        {
            _userService = userService;
            _folderService = folderService;
            _fileService = fileService;
            _sharedItemService = sharedItemService;
            _commentService = commentService;
        }
        public static IMenu CreateMenu(string menuType /*, User? user*/)
        {
            return menuType switch
            {
                "MainMenu" => new MainMenu(_userService, _folderService, _fileService, _sharedItemService, _commentService),
                //"LoginMenu" => new LoginMenu(_userService, user, _folderService, _fileService, _sharedItemService, _commentService)
            };
        }
        public static IMenu CreateLoginMenu(User user)
        {
            return new LoginMenu(_userService, user, _folderService, _fileService, _sharedItemService, _commentService);
        }
    }
}