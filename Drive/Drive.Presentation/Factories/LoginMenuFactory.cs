using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Actions.Disk;
using Drive.Presentation.Actions;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Menus.SubMenu;


namespace Drive.Presentation.Factories
{
    public class LoginMenuFactory
    {
        public static IAction CreateAction(string action, IUserService userService, User loggedUser, IFolderService folderService, IFileService fileService, ISharedItemService sharedItemService, ICommentService commentService)
        {
            return action switch
            {
                "Moj disk" => new UserDiskAction(userService, folderService, loggedUser, fileService, sharedItemService, commentService),
                "Dijeljeno sa mnom" => new UserSharedFilesAction(userService, loggedUser, sharedItemService, fileService, commentService),
                "Postavke Profila" => new UserProfileMenu(userService, loggedUser),
                "Odjava iz profila" => new LogOutAction(userService, folderService),
                _ => throw new ArgumentException($"Pogreska prilikom kreiranja akcije: {action}")
            };
        }
    }
}
