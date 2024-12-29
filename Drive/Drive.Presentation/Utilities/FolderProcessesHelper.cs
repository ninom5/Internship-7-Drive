using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Data.Enums;
using Drive.Domain.Repositories;
using Drive.Presentation.Reader;

namespace Drive.Presentation.Utilities
{
    public class FolderProcessesHelper
    {
        public static void DisplayFolder(Folder folder)
        {
            if (folder.ParentFolder != null)
            {
                Console.WriteLine($"\t- Mapa: {folder.Name}, Id mape: {folder.Id}, Parent folder id: {folder.ParentFolderId}, naziv: {folder.ParentFolder.Name}");
            }
            else
            {
                Console.WriteLine($"\t- Mapa: {folder.Name}, Id mape: {folder.Id}");
            }
        }
        public static void DeleteFolder(Folder folder, IFolderService folderService)
        {
            var deleteFolderStatus = folderService.DeleteFolder(folder);
            if (deleteFolderStatus != Domain.Enums.Status.Success)
            {
                Console.WriteLine($"Pogreska prilikom brisanja mape: {folder.Name}");
                return;
            }

            Console.WriteLine($"Uspjesno izbrisana mapa: {folder.Name}");
        }
        public static void ShareFolder(Folder folder, User user, User shareToUser, ISharedItemService sharedItemService)
        {
            if (sharedItemService.AlreadyShared(folder.Id, shareToUser.Id, user.Id, DataType.Folder))
            {
                Console.WriteLine($"Mapa: {folder.Name} je vec podijeljena s korisnikom: {shareToUser.Name}");
                return;
            }

            var shareFolderStatus = sharedItemService.Create(folder.Id, DataType.Folder, user, shareToUser, folder, null);
            if (shareFolderStatus != Domain.Enums.Status.Success)
            {
                Console.WriteLine($"Pogreska prilikom dijeljenja mape: {folder.Name}");
                return;
            }
            Console.WriteLine($"Mapa: {folder.Name} uspjesno podijeljena s korisnikom: {shareToUser.Name + " " + shareToUser.Email}");
        }
        public static void StopSharingFolder(Folder folder, User user, User shareToUser, ISharedItemService sharedItemService)
        {
            if (!sharedItemService.AlreadyShared(folder.Id, shareToUser.Id, user.Id, DataType.Folder))
            {
                Console.WriteLine($"Mapa: {folder.Name} nije ni podijeljena s korisnikom: {shareToUser.Name}");
                return;
            }

            var sharedFolder = sharedItemService.GetSharedItem(folder.Id, user, shareToUser, DataType.Folder);
            if (sharedFolder == null)
            {
                Console.WriteLine($"Pogreska prilikom dohvacanja mape: {folder.Name}");
                return;
            }

            var removeShareStatus = sharedItemService.Remove(sharedFolder);
            if (removeShareStatus != Domain.Enums.Status.Success)
            {
                Console.WriteLine($"Pogreska prilikom prestanka dijeljenja mape: {folder.Name}");
                return;
            }

            Console.WriteLine($"Uspjesno prekinuto dijeljenje mape: {folder.Name} s korisnikom: {shareToUser.Name}");
        }
        public static void RenameFolder(string currentName, string newName, IEnumerable<Folder> userFolders, IFolderService _folderService, User user)
        {
            var folderToRename = FolderRepository.GetFolder(userFolders, currentName);
            if (folderToRename == null)
            {
                Console.WriteLine("Folder s unesenim imenom nije pronaden");
                return;
            }

            while (true)
            {
                if (_folderService.GetFolderByName(newName, user) != null)
                {
                    Console.WriteLine("Folder s unesenim imenom vec postoji. Unesite novo ime ili ostavite prazno za odustajanje");
                    newName = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(newName))
                    {
                        Console.WriteLine("Odustajanje od preimenovanja mape.");
                        return;
                    }
                    continue;
                }

                var status = _folderService.UpdateFolder(folderToRename, newName);
                if (status != Domain.Enums.Status.Success)
                {
                    Console.WriteLine("Pogreska prilikom mijenjanja imena");
                    return;
                }

                break;
            }

            Console.WriteLine($"Mapa: {currentName} uspjesno preimenovana u: {newName}");
            ReadInput.WaitForUser();
        }
    }
}
