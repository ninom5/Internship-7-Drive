using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Data.Enums;

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
    }
}
