using Drive.Data.Entities.Models;
using Drive.Data.Enums;
using Drive.Domain.Interfaces.Services;
using Drive.Domain.Repositories;
using Drive.Domain.Services;
using Drive.Presentation.Menus.SubMenu;
using Drive.Presentation.Utilities;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace Drive.Presentation.Actions
{
    public class CommandAction
    {
        public static Folder currentFolder { get; private set; } = null;
        public static void CommandMode(User user, IFolderService _folderService, Folder parrentFolder, IFileService _fileService, IEnumerable<Folder> userFolders, IUserService _userService, ISharedItemService _sharedItemService)
        {
            currentFolder = parrentFolder;

            Console.Write("Unesite komandu za upravljanje datotekama i fileovima. Za pomoc unesite ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("help");

            Console.ResetColor();

            while (true)
            {
                Console.WriteLine("\n>");
                var command = Console.ReadLine();

                if (string.IsNullOrEmpty(command))
                    continue;

                if (command == "help")
                {
                    HelpMenu.DisplayHelp();
                    continue;
                }

                if (command == "povratak")
                    break;

                CheckCommand(command, user, _folderService, _fileService, userFolders, _userService, _sharedItemService);
                userFolders = _userService.GetFoldersOrFiles<Folder>(user);
            }
        }

        private static void CheckCommand(string command, User user, IFolderService _folderService, IFileService _fileService, IEnumerable<Folder> userFolders, IUserService _userService, ISharedItemService _sharedItemService)
        {
            Console.Clear();

            string[] parts = command.Split(" ");
            switch (parts[0])
            {
                case "stvori":
                    CreateFolderFile(parts, user, _folderService, _fileService);
                    break;

                case "udi":
                    ChangeWorkingDirectory(parts, userFolders);
                    break;

                case "uredi":
                    EditFile(parts, user, userFolders, _fileService, _userService);
                    break;

                case "izbrisi":
                    DeleteFolderFile(parts, user, _folderService, _fileService, _userService, userFolders);
                    break;

                case "promijeni":
                    RenameFolderFile(parts, userFolders, _folderService, _fileService, _userService, user);
                    break;

                case "trenutni_direktorij":
                    Console.WriteLine($"Trenutno se nalazite u mapi: {currentFolder.Name}");
                    break;

                case "podijeli":
                    StartSharing(parts, userFolders, _folderService, _fileService, _userService, _sharedItemService, user);
                    break;

                default:
                    Console.WriteLine("Pogresna komanda. Za pomoc unesite help");
                    break;
            }
        }
        private static void CreateFolderFile(string[] parts, User user, IFolderService _folderService, IFileService _fileService)
        {
            if (parts.Length < 3 || (parts[1] != "mapu" && parts[1] != "datoteku"))
            {
                Console.WriteLine("Ne ispravna komanda za stvaranje nove mape  datoteke. Za pomoc unesite help");
                return;
            }

            string dataType = parts[1];
            string name = GetName(parts.Skip(2));

            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Ime ne moze biti prazno");
                return;
            }

            if (dataType == "mapu")
            {
                Create<Folder>(name, currentFolder, user, _folderService, _fileService);
                return;
            }

            Create<Drive.Data.Entities.Models.File>(name, currentFolder, user, _folderService, _fileService);
        }
        private static void ChangeWorkingDirectory(string[] parts, IEnumerable<Folder> userFolders)
        {
            if (parts.Length < 4 || parts[1] != "u" || parts[2] != "mapu")
            {
                Console.WriteLine("Pogresan oblik komande za promjenu trenutne mape. Za pomoc unesite help");
                return;
            }

            string name = GetName(parts.Skip(3));
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Ime ne moze biti prazno");
                return;
            }

            var folder = FolderRepository.GetFolder(userFolders, name);
            if (folder == null)
            {
                Console.WriteLine("Nije pronaden folder s unesenim imenom");
                return;
            }

            currentFolder = folder;
            Console.WriteLine($"Trenutno unutar mape: {currentFolder.Name}");
        }
        private static void EditFile(string[] parts, User user, IEnumerable<Folder> userFolders, IFileService _fileService, IUserService _userService)
        {
            if (parts[1] != "datoteku")
            {
                Console.WriteLine("Pogrešna komanda. Za pomoć unesite help.");
                return;
            }

            var name = GetName(parts.Skip(2));
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Ime datoteke ne može biti prazno.");
                return;
            }

            var file = _userService.GetFoldersOrFiles<Drive.Data.Entities.Models.File>(user).FirstOrDefault(f => f.Name == name);
            if (file == null)
            {
                Console.WriteLine("Datoteka s unesenim imenom nije pronađena.");
                return;
            }

            Console.WriteLine($"----------Trenutni sadrzaj datoteke----------\n{file.Content}" +
                $"\n---------------------------------------------");

            List<string> newContent = new List<string>();
            var currentLine = "";
            bool isSaved = false;

            while (true)
            {
                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                {
                    if (currentLine.StartsWith(":"))
                    {
                        currentLine = currentLine.Substring(1, currentLine.Length - 1).Trim();

                        if (currentLine == "spremanje i izlaz")
                        {
                            Console.WriteLine("\nSpremanje promjena...\nIzlaz...");
                            file.Content = string.Join(Environment.NewLine, newContent);

                            file.LastModifiedAt = DateTime.UtcNow;

                            _fileService.UpdateFileContent(file);

                            isSaved = true;

                            break;
                        }
                        else if (currentLine == "izlaz bez spremanja")
                        {
                            Console.WriteLine("\nNece se spremiti nista.\nIzlaz...");
                            break;
                        }
                        else if(currentLine == "help")
                        {
                            Console.WriteLine("\n\t- :spremanje i izlaz spremanje promjena i izlaz \n\t- :izlaz bez spremanja odbacivanje promjena i izlaz\n");
                            currentLine = "";
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("\nNe ispravna komanda. Unesite help za pomoc\n");
                            currentLine = "";
                            continue;
                        }
                    }

                    newContent.Add(currentLine);
                    Console.WriteLine();
                    currentLine = "";
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (currentLine.Length > 0)
                    {
                        currentLine = currentLine.Substring(0, currentLine.Length - 1);
                        Console.Write("\b \b");
                    }
                    else if (newContent.Count > 0)
                    {
                        currentLine = newContent[^1];
                        newContent.RemoveAt(newContent.Count - 1);

                        Console.CursorTop--;
                        Console.CursorLeft = 0;
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.CursorLeft = 0;
                        Console.Write(currentLine);
                    }
                }
                else if(key.Key == ConsoleKey.Z && key.Modifiers == ConsoleModifiers.Control)
                {
                    Console.WriteLine();
                    break;
                }
                else
                {
                    currentLine += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }

            if (isSaved)
            {
                Console.WriteLine("Novi sadrzaj: ");
                foreach (var line in newContent)
                {
                    Console.WriteLine(line);
                }
            }
        }
        private static void DeleteFolderFile(string[] parts, User user, IFolderService _folderService, IFileService _fileService, IUserService _userService, IEnumerable<Folder> userFolders)
        {
            if (parts.Length < 3 || (parts[1] != "mapu" && parts[1] != "datoteku"))
            {
                Console.WriteLine("Pogresan oblik komande za promjenu trenutne mape. Za pomoc unesite help");
                return;
            }

            string dataType = parts[1];
            string name = GetName(parts.Skip(2));

            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Ime ne moze biti prazno");
                return;
            }

            if (dataType == "mapu")
            {
                var folderToDelete = FolderRepository.GetFolder(userFolders, name);
                if (folderToDelete == null)
                {
                    Console.WriteLine("Nije pronaden folder s tim imenom");
                    return;
                }

                if (folderToDelete.Name == "Root Folder" || folderToDelete == currentFolder || Helper.IsAncestor(currentFolder, name, userFolders))
                {
                    Console.WriteLine("Ne mozete izbrisati root folder, folder u kojem se trenutno nalazite ili nadmapu trenutnog foldera");
                    return;
                }

                ProcessFolderAndContents(folderToDelete, userFolders, _folderService, _fileService, _userService, user, "izbrisi", null, null);

                return;
            }


            var fileToDelete = _userService.GetFoldersOrFiles<Drive.Data.Entities.Models.File>(user).FirstOrDefault(f => f.Name == name);
            if (fileToDelete == null)
            {
                Console.WriteLine("Nije pronaden zeljeni file");
                return;
            }

            var deleteFileStatus = _fileService.DeleteFile(fileToDelete);
            if (deleteFileStatus != Domain.Enums.Status.Success)
            {
                Console.WriteLine("pogreska prilikom brisanja filea");
                return;
            }

            Console.WriteLine($"Datoteka {fileToDelete.Name} s id: {fileToDelete.Id} uspjesno obrisana");
        }

        private static void RenameFolderFile(string[] parts, IEnumerable<Folder> userFolders, IFolderService _folderService, IFileService _fileService, IUserService _userService, User user)
        {
            if (parts.Length < 6 || (parts[2] != "mape" && parts[2] != "datoteke"))
            {
                Console.WriteLine("Pogresna komanda. Za pomoc unesite help");
                return;
            }

            string pattern = @"'([^']*)' u '([^']*)'";
            string fullName = string.Join(" ", parts.Skip(2));

            Match match = Regex.Match(fullName, pattern);
            if (!match.Success)
            {
                Console.WriteLine("pogreska prilikom dohvacanja imena. Za pomoc unesite help");
                return;
            }

            string currentName = match.Groups[1].Value.Trim();
            string newName = match.Groups[2].Value.Trim();

            if (currentName == "Root Folder")
            {
                Console.WriteLine("Root folder se ne moze preimenovati");
                return;
            }

            if (string.IsNullOrEmpty(newName))
            {
                Console.WriteLine("Ime ne moze biti prazno");
                return;
            }

            if (parts[2] == "mape")
            {

                var folderToRename = FolderRepository.GetFolder(userFolders, currentName);
                if (folderToRename == null)
                {
                    Console.WriteLine("Folder s unesenim imenom nije pronaden");
                    return;
                }

                var status = _folderService.UpdateFolder(folderToRename, newName);
                if (status != Domain.Enums.Status.Success)
                {
                    Console.WriteLine("Pogreska prilikom mijenjanja imena");
                    return;
                }

                Console.WriteLine($"Mapa: {currentName} uspjesno preimenovana u: {newName}");

                return;
            }

            var fileToRename = _userService.GetFoldersOrFiles<Drive.Data.Entities.Models.File>(user).Where(f => f.Name == currentName).FirstOrDefault();
            if (fileToRename == null)
            {
                Console.WriteLine("Nije pronaden zeljeni file");
                return;
            }

            var renameStatus = _fileService.UpdateFile(fileToRename, newName);

            if (renameStatus != Domain.Enums.Status.Success)
            {
                Console.WriteLine("Pogreska prilikom mijenjanja imena");
                return;
            }
            Console.WriteLine($"Uspjesno promijenjen naziv datoteke: {currentName} u: {newName}");
        }

        private static string? GetName(IEnumerable<string> parts)
        {
            string fullInput = string.Join(" ", parts);
            string pattern = @"'([^']*)'";

            Match match = Regex.Match(fullInput, pattern);

            return match.Success ? match.Groups[1].Value.Trim() : null;
        }
        public static void StartSharing(string[] parts, IEnumerable<Folder> userFolders, IFolderService _folderService, IFileService _fileService, IUserService _userService, ISharedItemService _sharedItemService, User user)
        {
            if (parts.Length < 4 || (parts[1] != "mapu" && parts[1] != "datoteku") || parts[2] != "s")
            {
                Console.WriteLine("Ne ispravan oblik komande Podijeli. Za pomoc unesite help");
                return;
            }

            var email = GetName(parts.Skip(3));
            if (email == null) 
            {
                Console.WriteLine("Email ne moze biti prazan. Povratak...");
                return;
            }

            if (!_userService.EmailExists(email))
            {
                Console.WriteLine("Uneseni email ne postoji.");
                return;
            }

            var userToShare = _userService.GetUser(email);
            if(userToShare == null)
            {
                Console.WriteLine("Uneseni korisnik nije pronaden");
                return;
            }

            if (parts[1] == "mapu")
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

                    var folder = _userService.GetFoldersOrFiles<Folder>(user).Where(f => f.Name == folderName).FirstOrDefault();

                    if (folder == null)
                    {
                        Console.WriteLine("Nije pronaden zelejni folder. Pokusajte opet");
                        continue;
                    }

                    ProcessFolderAndContents(folder, userFolders, _folderService, _fileService, _userService, user, "podijeli", _sharedItemService, userToShare);

                    return;
                }
            }
        }

        public static void Create<T>(string name, Folder folder, User user, IFolderService? _folderService, IFileService? _fileService)
        {
            if (typeof(T) == typeof(Folder))
            {
                if (_folderService == null)
                {
                    Console.WriteLine("pogreska prilikom kreiranja foldera");
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
            else if (typeof(T) == typeof(Drive.Data.Entities.Models.File))
            {
                Console.WriteLine("Unesite sadrzaj datoteke");
                StringBuilder stringBuilder = new StringBuilder();

                while (true)
                {
                    var lineOfContent = Console.ReadLine();
                    if (string.IsNullOrEmpty(lineOfContent))
                        break;

                    stringBuilder.AppendLine(lineOfContent);
                }

                var content = stringBuilder.ToString();
                if (string.IsNullOrEmpty(content))
                {
                    Console.WriteLine("Sadrzaj ne moze biti prazan. Povratak...");
                    return;
                }

                if(_fileService == null)
                {
                    Console.WriteLine("Pogreska prilikom kreiranja filea");
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
        private static void ProcessFolderAndContents(Folder folder, IEnumerable<Folder> allFolders, IFolderService _folderService, IFileService _fileService, IUserService _userService, User user, 
            string process, ISharedItemService? _sharedItemService, User? shareToUser)
        {
            var subFolders = allFolders.Where(f => f.ParentFolderId == folder.Id).ToList();

            foreach (var subFolder in subFolders)
                ProcessFolderAndContents(subFolder, allFolders, _folderService, _fileService, _userService, user, process, _sharedItemService, shareToUser);

            var filesInFolder = _userService.GetFoldersOrFiles<Drive.Data.Entities.Models.File>(user).Where(file => file.FolderId == folder.Id).ToList();

            foreach (var file in filesInFolder)
            {
                if (process == "izbrisi")
                {
                    var deleteFileStatus = _fileService.DeleteFile(file);
                    if (deleteFileStatus != Domain.Enums.Status.Success)
                    {
                        Console.WriteLine($"Pogreska prilikom brisanja datoteke: {file.Name}");
                        return;
                    }

                    Console.WriteLine($"Uspješno izbrisana datoteka: {file.Name} u mapi: {folder.Name}");
                }
                else if (process == "podijeli" && _sharedItemService != null && shareToUser != null)
                {
                    var shareFileStatus = _sharedItemService.Create(file.Id, DataType.File, user, shareToUser);

                    if (shareFileStatus != Domain.Enums.Status.Success)
                    {
                        Console.WriteLine($"Pogreska prilikom dijeljenja datoteke: {file.Name}");
                        continue;
                    }

                    Console.WriteLine($"Datoteka: {file.Name} uspjesno podijeljena s korisnikom: {shareToUser.Name + " " +  shareToUser.Email}");
                }
            }

            if (process == "izbrisi")
            {
                var deleteFolderStatus = _folderService.DeleteFolder(folder);
                if (deleteFolderStatus != Domain.Enums.Status.Success)
                {
                    Console.WriteLine($"Pogreska prilikom brisanja mape: {folder.Name}");
                }

                Console.WriteLine($"Uspjesno izbrisana mapa: {folder.Name}");
            }
            else if(process == "podijeli" && _sharedItemService != null && shareToUser != null)
            {
                var shareFolderStatus = _sharedItemService.Create(folder.Id, DataType.Folder, user, shareToUser);

                if (shareFolderStatus != Domain.Enums.Status.Success)
                {
                    Console.WriteLine($"Pogreska prilikom dijeljenja mape: {folder.Name}");
                    return;
                }

                Console.WriteLine($"Mapa: {folder.Name} uspjesno podijeljena s korisnikom: {shareToUser.Name + " " + shareToUser.Email}");
            }
        }
    }
}