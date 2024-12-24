using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Domain.Repositories;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Utilities;

namespace Drive.Presentation.Actions
{
    public class UserDiskAction : IAction
    {
        private readonly IUserService _userService;
        private readonly IFolderService _folderService;
        private readonly User _user;
        private readonly IFileService _fileService;
        private readonly ISharedItemService _sharedItemService;
        public UserDiskAction(IUserService userService, IFolderService folderService, User user, IFileService fileService, ISharedItemService sharedItemService)
        {
            _userService = userService;
            _folderService = folderService;
            _user = user;
            _fileService = fileService;
            _sharedItemService = sharedItemService;
        }

        public void Execute()
        {
            Console.Clear();

            var userFolders = _userService.GetFoldersOrFiles<Folder>(_user);
            var userFiles = _userService.GetFoldersOrFiles<Drive.Data.Entities.Models.File>(_user);

            if (!userFolders.Any() && !userFiles.Any())
                Console.WriteLine("Nemate kreiranih mapa i datoteka");


            Console.WriteLine("Vase datoteke: ");

            foreach (var folder in userFolders.OrderBy(folder => folder.Name))
            {
                Helper.DisplayFolder(folder);
                Helper.DisplayFilesForFolder(userFiles, folder.Id);
                Console.WriteLine();
            }

            Console.WriteLine("Za ulazak u komandni nacin pritisnite bilo koju tipku");
            Console.ReadKey();

            var rootFolder = FolderRepository.GetFolder(userFolders);
            if (rootFolder == null)
            {
                Console.WriteLine("Pogreska pri pronalasku root foldera");
                return;
            }

            CommandAction.CommandMode(_user, _folderService, rootFolder, _fileService, userFolders, _userService, _sharedItemService);
        }
    }
}
