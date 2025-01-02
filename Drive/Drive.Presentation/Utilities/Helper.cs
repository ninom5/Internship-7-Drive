using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Reader;
using System.Text.RegularExpressions;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Utilities
{
    public class Helper
    {
        public static string CollectMail(IUserService _userService)
        {
            while (true)
            {
                var email = ReadInput.ReadString("Unesite email: ", input => ReadInput.IsValidEmail(input), "Email mora biti u formatu [string min 1 chara]@[string min 2 chara].[string min 3 chara]\n");

                if (email == null)
                {
                    Console.WriteLine("Email ne moze biti prazan. Povratak...");
                    return "";
                }

                if (!_userService.EmailExists(email))
                    return email;

                Console.WriteLine("Uneseni email vec postoji");
            }
        }
        
        public static bool IsAncestor(Folder currentFolder, string folderToDeleteName, IEnumerable<Folder> allFolders)
        {
            while (currentFolder.ParentFolderId != null)
            {
                var parentFolder = allFolders.FirstOrDefault(f => f.Id == currentFolder.ParentFolderId);

                if (parentFolder == null)
                    return false;

                if (parentFolder.Name == folderToDeleteName)
                    return true;

                currentFolder = parentFolder;
            }

            return false;
        }
       
        public static void ShowUserFoldersAndFiles(User user, IUserService _userService, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles)
        {
            if (!userFolders.Any() && !userFiles.Any())
                Console.WriteLine("Nemate kreiranih mapa i datoteka");


            Console.WriteLine("Vase datoteke: ");

            foreach (var folder in userFolders.OrderBy(folder => folder.Name))
            {
                FolderProcessesHelper.DisplayFolder(folder);
                FileProcessesHelper.DisplayFilesForFolder(userFiles, folder.Id);
                Console.WriteLine();
            }
        }
        public static (IEnumerable<Folder>, IEnumerable<File>) ShowSharedDataWithUser(ISharedItemService _sharedItemService, User _loggedUser)
        {
            var sharedFolders = GetSharedFolders(_sharedItemService, _loggedUser);
            var sharedFiles = GetSharedFiles(_sharedItemService, _loggedUser);

            return (sharedFolders, sharedFiles);
        }

        private static IEnumerable<Folder> GetSharedFolders(ISharedItemService _sharedItemService, User _loggedUser)
        {
            var sharedFolders = _sharedItemService.GetAllSharedWithUser(_loggedUser, Data.Enums.DataType.Folder).OrderBy(f => f.Folder.Name);
            var folders = new List<Folder>();

            Console.WriteLine("Podijeljene mape s vama: ");

            if (!sharedFolders.Any())
                Console.WriteLine("\tNema mapa podijeljenih s vama");
            else
            {
                foreach (var folder in sharedFolders)
                {
                    if (folder != null)
                    {
                        Console.WriteLine($"\t-Mapa: {folder.Folder.Name} id mape: {folder.Folder.Id} podijeljena od korisnika: {folder.Folder.Owner.Name}");
                        folders.Add(folder.Folder);
                    }
                }
            }

            return folders;
        }

        private static IEnumerable<File> GetSharedFiles(ISharedItemService _sharedItemService, User _loggedUser)
        {
            var sharedFiles = _sharedItemService.GetAllSharedWithUser(_loggedUser, Data.Enums.DataType.File).OrderBy(f => f.File.LastModifiedAt);
            var files = new List<File>();

            Console.WriteLine("\nPodijeljene datoteke s vama: ");

            if (!sharedFiles.Any())
                Console.WriteLine("\tNema datoteka podijeljenih s vama");
            else
            {
                foreach (var file in sharedFiles)
                {
                    if (file != null)
                    {
                        Console.WriteLine($"\t-Datoteka: {file.File.Name} podijeljena od korisnika: {file.File.Owner.Name} unutar mape: {file.File.Folder.Name}," +
                            $" zadnji put promijenjena: {file.File.LastModifiedAt}");
                        files.Add(file.File);
                    }
                }
            }

            return files;
        }


        public static User? ValidStartSharingCommandAndUser(string[] parts, IUserService _userService, User user)
        {
            if (!IsValidShareCommand(parts))
            {
                Console.WriteLine("Ne ispravan oblik komande Podijeli. Za pomoc unesite help");
                return null;
            }

            var email = GetEmailFromParts(parts);
            if (email == null)
                return null;

            if (email == user.Email)
            {
                Console.WriteLine("Ne mozete dijeliti datoteku ili mapu sa samim sobom");
                return null;
            }

            return GetUserToShareWith(email, _userService);
        }

        private static bool IsValidShareCommand(string[] parts)
        {
            return parts.Length == 4 && (parts[1] == "mapu" || parts[1] == "datoteku") && parts[2] == "s";
        }

        private static string? GetEmailFromParts(IEnumerable<string> parts)
        {
            var email = GetName(parts.Skip(3));
            if (email == null)
                Console.WriteLine("Email ne moze biti prazan. Povratak...");

            return email;
        }

        private static User? GetUserToShareWith(string email, IUserService _userService)
        {
            if (!_userService.EmailExists(email))
            {
                Console.WriteLine("Uneseni email ne postoji.");
                return null;
            }

            var userToShare = _userService.GetUser(email);
            if (userToShare == null)
            {
                Console.WriteLine("Uneseni korisnik nije pronaden.");
                return null;
            }

            return userToShare;
        }


        public static void ProcessFolderAndContents(Folder folder, IEnumerable<Folder> allFolders, IFolderService _folderService, IFileService _fileService, IUserService _userService, User user,
            string process, ISharedItemService? _sharedItemService, User? shareToUser)
        {
            Console.Clear();

            var subFolders = allFolders.Where(f => f.ParentFolderId == folder.Id).ToList();

            foreach (var subFolder in subFolders)
                ProcessFolderAndContents(subFolder, allFolders, _folderService, _fileService, _userService, user, process, _sharedItemService, shareToUser);

            var filesInFolder = _userService.GetFoldersOrFiles<File>(user).Where(file => file.FolderId == folder.Id).ToList();

            foreach (var file in filesInFolder)
            {
                switch (process)
                {
                    case "izbrisi":
                        FileProcessesHelper.DeleteFile(file, folder, _fileService);
                        break;

                    case "podijeli":
                        if (shareToUser != null && _sharedItemService != null)
                            FileProcessesHelper.ShareFile(file, user, shareToUser, _sharedItemService);
                        break;

                    case "prestani dijeliti":
                        if (shareToUser != null && _sharedItemService != null)
                            FileProcessesHelper.StopSharingFile(file, user, shareToUser, _sharedItemService);
                        break;

                    default:
                        Console.WriteLine("Pogreska od strane aplikacije");
                        break;
                }

            }

            switch (process)
            {
                case "izbrisi":
                    FolderProcessesHelper.DeleteFolder(folder, _folderService);
                    break;

                case "podijeli":
                    if (shareToUser != null && _sharedItemService != null)
                        FolderProcessesHelper.ShareFolder(folder, user, shareToUser, _sharedItemService);
                    break;

                case "prestani dijeliti":
                    if (shareToUser != null && _sharedItemService != null)
                        FolderProcessesHelper.StopSharingFolder(folder, user, shareToUser, _sharedItemService);
                    break;

                default:
                    Console.WriteLine($"Pogreska od strane aplikacije");
                    break;
            }
        }
        public static User? EmailUserValid(string[] parts, IUserService _userService)
        {
            var email = Helper.GetName(parts.Skip(3));
            if (email == null)
            {
                Console.WriteLine("Email ne moze biti prazan. Povratak...");
                return null;
            }

            return ReadInput.FindUser(_userService, email);
        }

        public static (string, string) CheckRenameCommand(string[] parts)
        {
            if (parts.Length < 6 || parts[1] != "naziv" || (parts[2] != "mape" && parts[2] != "datoteke"))
            {
                Console.WriteLine("Pogresna komanda. Za pomoc unesite help");
                return ("", "");
            }

            string pattern = @"'([^']*)' u '([^']*)'";
            string fullName = string.Join(" ", parts.Skip(2));

            Match match = Regex.Match(fullName, pattern);
            if (!match.Success)
            {
                Console.WriteLine("pogreska prilikom dohvacanja imena. Za pomoc unesite help");
                return ("", "");
            }

            return (match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim());
        }

        public static void Create<T>(string name, Folder folder, User user, IFolderService _folderService, IFileService _fileService)
        {
            if (typeof(T) == typeof(Folder))
            {
                if (!ReadInput.ConfirmAction($"Zelite li napraviti novi {typeof(T).Name} "))
                {
                    Console.WriteLine("Odustali ste od kreiranja");
                    return;
                }

                var creatingFolderStatus = _folderService.CreateFolder(name, user, folder);
                if (creatingFolderStatus != Domain.Enums.Status.Success)
                {
                    Console.WriteLine("Pogreska prilikom kreiranja foldera");
                    return;
                }

                Console.WriteLine($"Folder: {name} uspjesno kreiran unutar mape: {folder.Name}");

            }
            else if (typeof(T) == typeof(File))
            {
                var content = ReadInput.ReadFileContent();
                if (string.IsNullOrEmpty(content))
                    return;

                if (!ReadInput.ConfirmAction($"Zelite li napraviti novi {typeof(T).Name} "))
                {
                    Console.WriteLine("Odustali ste od kreiranja");
                    return;
                }

                var creatingFileStatus = _fileService.CreateFile(name, content, user, folder);
                if (creatingFileStatus != Domain.Enums.Status.Success)
                {
                    Console.WriteLine("Pogreska prilikom kreiranja datoteke");
                    return;
                }

                Console.WriteLine($"Datoteka: {name} uspjesno kreirana unutar mape: {folder.Name}");
            }
        }
        public static string? GetName(IEnumerable<string> parts)
        {
            string fullInput = string.Join(" ", parts);
            string pattern = @"'([^']*)'";

            Match match = Regex.Match(fullInput, pattern);

            return match.Success ? match.Groups[1].Value.Trim() : null;
        }
        public static bool IsValidCommandStopSharing(string[] parts)
        {
            return parts.Length == 5 && parts[1] == "dijeliti" && (parts[2] == "mapu" || parts[2] == "datoteku") && parts[3] == "s";
        }
    }
}
