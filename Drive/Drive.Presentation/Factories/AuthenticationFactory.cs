using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Actions.Authentication;
using Drive.Presentation.Interfaces;

namespace Drive.Presentation.Factories
{
    public class AuthenticationFactory
    {
        public static IAction CreateAction(string action, IUserService userService, IFolderService folderService, IFileService fileService, ISharedItemService sharedItemService, ICommentService commentService)
        {
            return action switch
            {
                "LoginAction" => new LoginAction(userService, folderService, fileService, sharedItemService, commentService),
                "RegisterUserAction" => new RegisterUserAction(userService, folderService),
                _ => throw new ArgumentException($"Pogreska prilikom kreiranja akcije: {action}")
            };
        }
    }
}
