using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Data.Enums;

namespace Drive.Presentation.Utilities
{
    public class FileProcessesHelper
    {
        public static void DisplayFilesForFolder(IEnumerable<Drive.Data.Entities.Models.File> files, int folderId)
        {
            var folderFiles = files
                .Where(file => file.FolderId == folderId)
                .OrderBy(file => file.LastModifiedAt);

            foreach (var file in folderFiles)
            {
                Console.WriteLine($"\tFile: {file.Name}, id mape: {file.FolderId}, zadnji put promijenjeno: {file.LastModifiedAt}");
            }
        }
        public static void DeleteFile(Drive.Data.Entities.Models.File file, Folder folder, IFileService fileService)
        {
            var deleteFileStatus = fileService.DeleteFile(file);
            if (deleteFileStatus != Domain.Enums.Status.Success)
            {
                Console.WriteLine($"pogreska prilikom brisanja datoteke: {file.Name}");
                return;
            }
            Console.WriteLine($"Uspjesno izbrisana datoteka: {file.Name} u mapi: {folder.Name}");
        }
        public static void ShareFile(Drive.Data.Entities.Models.File file, User user, User shareToUser, ISharedItemService sharedItemService)
        {
            if (sharedItemService.AlreadyShared(file.Id, shareToUser.Id, user.Id, DataType.File))
            {
                Console.WriteLine($"Datoteka '{file.Name}' je ves podijeljena s korisnikom: {shareToUser.Name}");
                return;
            }

            var shareFileStatus = sharedItemService.Create(file.Id, DataType.File, user, shareToUser, null, file);
            if (shareFileStatus != Domain.Enums.Status.Success)
            {
                Console.WriteLine($"Pogreska prilikom dijeljenja datoteke: {file.Name}");
                return;
            }
            Console.WriteLine($"Datoteka: {file.Name} uspjesno podijeljena s korisnikom: {shareToUser.Name}");
        }

        public static void StopSharingFile(Drive.Data.Entities.Models.File file, User user, User shareToUser, ISharedItemService sharedItemService)
        {
            if (!sharedItemService.AlreadyShared(file.Id, shareToUser.Id, user.Id, DataType.File))
            {
                Console.WriteLine($"Datoteka '{file.Name}' nije ni podijeljena s korisnikom: {shareToUser.Name}");
                return;
            }

            var sharedItem = sharedItemService.GetSharedItem(file.Id, user, shareToUser, DataType.File);
            if (sharedItem == null)
            {
                Console.WriteLine($"Pogreska prilikom dohvacanja datoteke: {file.Name}");
                return;
            }

            var removeShareStatus = sharedItemService.Remove(sharedItem);
            if (removeShareStatus != Domain.Enums.Status.Success)
            {
                Console.WriteLine($"Pogreska prilikom prestanka dijeljenja datoteke: {file.Name}");
                return;
            }

            Console.WriteLine($"Uspjesno prekinuto dijeljenje datoteke: {file.Name} s korisnikom: {shareToUser.Name + " " + shareToUser.Email}");
        }

    }
}
