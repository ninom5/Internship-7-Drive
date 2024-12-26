using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Interfaces;

namespace Drive.Presentation.Actions
{
    public class UserSharedFilesAction : IAction
    {
        private readonly IUserService _userService;
        private readonly ISharedItemService _sharedItemService;
        private readonly User _LoggedUser;

        public UserSharedFilesAction(IUserService userService, User user, ISharedItemService sharedItemService)
        {
            _userService = userService;
            _sharedItemService = sharedItemService;
            _LoggedUser = user;
        }

        public void Execute()
        {
            Console.Clear();

            var folders = _sharedItemService.GetAllSharedWithUser(_LoggedUser, Data.Enums.DataType.Folder);
            var files = _sharedItemService.GetAllSharedWithUser(_LoggedUser, Data.Enums.DataType.File);

            foreach (var folder in folders)
            {
                if (folder != null)
                {
                    Console.WriteLine($"mapa: {folder.Folder.Name}");
                }
            }
            foreach (var file in files)
            {
                if (file != null)
                {
                    Console.WriteLine($"Datoteka: {file.File.Name}");
                }
            }
        }
    }
}
