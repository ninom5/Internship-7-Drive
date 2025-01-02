using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Domain.Repositories;
using Drive.Presentation.Actions.Command;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Utilities;

namespace Drive.Presentation.Actions.Disk
{
    public class UserDiskAction : IAction
    {
        private readonly IUserService _userService;
        private readonly IFolderService _folderService;
        private readonly User _user;
        private readonly IFileService _fileService;
        private readonly ISharedItemService _sharedItemService;
        private readonly ICommentService _commentService;
        public UserDiskAction(IUserService userService, IFolderService folderService, User user, IFileService fileService, ISharedItemService sharedItemService, ICommentService commentService)
        {
            _userService = userService;
            _folderService = folderService;
            _user = user;
            _fileService = fileService;
            _sharedItemService = sharedItemService;
            _commentService = commentService;
        }

        public void Execute()
        {
            Console.Clear();

            var userFolders = _userService.GetFoldersOrFiles<Folder>(_user);
            var userFiles = _userService.GetFoldersOrFiles<Data.Entities.Models.File>(_user);

            Helper.ShowUserFoldersAndFiles(_user, _userService, userFolders, userFiles);


            Console.WriteLine("Za ulazak u komandni nacin pritisnite bilo koju tipku");
            Console.ReadKey();

            var rootFolder = FolderRepository.GetFolder(userFolders);
            if (rootFolder == null)
            {
                Console.WriteLine("Pogreska pri pronalasku root foldera");
                return;
            }

            CommandAction commandAction = new CommandAction();
            commandAction.CommandMode(_user, rootFolder, _folderService, _fileService, _userService, _sharedItemService, userFolders, userFiles, _commentService);
        }
    }
}