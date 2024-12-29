using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Reader;
using Drive.Data.Enums;
using File = Drive.Data.Entities.Models.File;
using Drive.Domain.Services;

namespace Drive.Presentation.Utilities
{
    public class SharedItemsProcesses
    {
        public static void StartSharingFolder(User user, User userToShare, IFolderService _folderService, ISharedItemService _sharedItemService, IUserService _userService, IFileService _fileService, IEnumerable<Folder> userFolders, ICommentService _commentService)
        {
            while (true)
            {
                Console.WriteLine("Unesite ime mape koju zelite podijeliti");
                var folderName = Console.ReadLine();

                if (string.IsNullOrEmpty(folderName))
                {
                    Console.WriteLine("Ime ne moze biti prazno. Povratak...");
                    return;
                }

                var folder = _folderService.GetFolderByName(folderName, user);

                if (folder == null)
                {
                    Console.WriteLine("Nije pronaden zelejni folder. Pokusajte opet");
                    continue;
                }

                if (!ReadInput.ConfirmAction($"Zelite li stvarno podijeliti mapu: {folder.Name} s korisnikom: {userToShare.Name} "))
                {
                    Console.WriteLine("Odustali ste od dijeljenja mape");
                    return;
                }

                Helper.ProcessFolderAndContents(folder, userFolders, _folderService, _fileService, _userService, user, "podijeli", _sharedItemService, userToShare);

                ReadInput.WaitForUser();

                return;
            }
        }
        public static void StartSharingFile(User user, User userToShare, ISharedItemService _sharedItemService, IUserService _userService)
        {
            while (true)
            {
                Console.WriteLine("Unesite ime datoteke koju zelite podijeliti (prazno za odustat)");

                var fileName = Console.ReadLine();

                if (string.IsNullOrEmpty(fileName))
                {
                    Console.WriteLine("Ime ne moze biti prazno. Povratak...");
                    return;
                }

                var file = _userService.GetFoldersOrFiles<File>(user).Where(f => f.Name == fileName).FirstOrDefault();

                if (file == null)
                {
                    Console.WriteLine("Nije pronaden zelejni file. Pokusajte opet");
                    continue;
                }

                if (_sharedItemService.AlreadyShared(file.Id, userToShare.Id, user.Id, DataType.File))
                {
                    Console.WriteLine($"ova datoteka je vec podijeljena s korisnikom: {userToShare.Name}");
                    continue;
                }

                if (!ReadInput.ConfirmAction($"Zelite li stvarno podijeliti datoteku: {file.Name} s korisnikom: {userToShare.Name} "))
                {
                    Console.WriteLine("Odustali ste od dijeljenja datoteke");
                    return;
                }

                var status = _sharedItemService.Create(file.Id, DataType.File, user, userToShare, null, file);

                if (status == Domain.Enums.Status.Failed)
                {
                    Console.WriteLine($"Pogreska prilikom dijeljenja datoteke: {file.Name}");
                    return;
                }

                Console.WriteLine($"Datoteka: {file.Name} uspjesno podijeljena s korisnikom: {userToShare.Name + " " + userToShare.Email}");

                ReadInput.WaitForUser();

                return;
            }
        }
        public static void HandleStopSharingFolder(User user, User userToShare, IFolderService _folderService, IFileService _fileService, IUserService _userService, ISharedItemService _sharedItemService, ICommentService _commentService)
        {
            while (true)
            {
                Console.WriteLine("Unesite ime mape koju zelite prestati dijeliti");
                var folderName = Console.ReadLine();

                if (string.IsNullOrEmpty(folderName))
                {
                    Console.WriteLine("Ime ne moze biti prazno. Povratak...");
                    return;
                }

                var userFolders = _userService.GetFoldersOrFiles<Folder>(user);
                var folder = userFolders.FirstOrDefault(f => f.Name == folderName);

                if (folder == null)
                {
                    Console.WriteLine("Nije pronaden zelejni folder. Pokusajte opet");
                    continue;
                }

                if (!ReadInput.ConfirmAction($"Zelite li stvarno prestati dijeliti mapu s korisnikom: {userToShare.Name} "))
                {
                    Console.WriteLine("Odustali ste od akcije");
                    return;
                }

                Helper.ProcessFolderAndContents(folder, userFolders, _folderService, _fileService, _userService, user, "prestani dijeliti", _sharedItemService, userToShare);

                ReadInput.WaitForUser();
                return;
            }
        }
        public static void HandleStopSharingFile(User user, User userToShare, IFileService _fileService, ISharedItemService _sharedItemService, ICommentService _commentService)
        {
            while (true)
            {
                Console.WriteLine("Unesite ime datoteke koju zelite prestati dijeliti (prazno za odustat)");

                var fileName = Console.ReadLine();

                if (string.IsNullOrEmpty(fileName))
                {
                    Console.WriteLine("Ime ne moze biti prazno. Povratak...");
                    return;
                }

                var file = _fileService.GetFileByName(fileName, user);

                if (file == null)
                {
                    Console.WriteLine("Nije pronaden zelejni file. Pokusajte opet");
                    continue;
                }

                RemoveFileIfShared(file, user, userToShare, _sharedItemService, _commentService);

                ReadInput.WaitForUser();
                return;
            }
        }
        public static void RemoveFileIfShared(File file, User user, User userToShare, ISharedItemService _sharedItemService, ICommentService commentService)
        {
            if (!_sharedItemService.AlreadyShared(file.Id, userToShare.Id, user.Id, DataType.File))
            {
                Console.WriteLine($"Ova datoteka nije ni podijeljena s korisnikom: {userToShare.Name}");
                return;
            }

            var sharedItem = _sharedItemService.GetSharedItem(file.Id, user, userToShare, DataType.File);

            if (!ReadInput.ConfirmAction($"zelite li prestati dijeliti datoteku s korisnikom: {userToShare.Name} "))
            {
                Console.WriteLine("odustali ste od akcije");
                return;
            }
            
            var commentsFromUserToShare = commentService.GetCommentsByFile(file).Where(item => item.UserId == userToShare.Id);
            foreach ( var comment in commentsFromUserToShare )
            {
                var removeCommentStatus = commentService.RemoveComment(comment);
                if(removeCommentStatus == Domain.Enums.Status.Failed)
                {
                    Console.WriteLine($"pogreska prilikom brisanja komentara: {comment.Id} korisnika {userToShare}");
                }
                else
                    Console.WriteLine($"uspjesno brisanja komentara: {comment.Id} korisnika {userToShare}");
            }

            var status = _sharedItemService.Remove(sharedItem);

            if (status == Domain.Enums.Status.Failed)
            {
                Console.WriteLine($"Pogreska prilikom prestanka dijeljenja datoteke: {file.Name}");
                return;
            }


            Console.WriteLine($"Uspjesno prekinuto dijeljenje datoteke: {file.Name} s korisnikom: {userToShare.Name + " " + userToShare.Email}");
            ReadInput.WaitForUser();
        }
    }
}
