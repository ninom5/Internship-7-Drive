﻿using Drive.Data.Entities.Models;
using Drive.Data.Enums;
using Drive.Domain.Interfaces.Services;
using Drive.Domain.Repositories;
using Drive.Presentation.Menus.SubMenu;
using Drive.Presentation.Reader;
using Drive.Presentation.Utilities;
using System.Text;
using System.Text.RegularExpressions;
using File = Drive.Data.Entities.Models.File;


namespace Drive.Presentation.Actions
{
    public class CommandAction
    {
        public static Folder currentFolder { get; private set; } = null;
        public void CommandMode(User user, Folder parrentFolder, IFolderService _folderService, IFileService _fileService, IUserService _userService, ISharedItemService _sharedItemService, 
            IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, ICommentService _commentService)
        {
            currentFolder = parrentFolder;

            Console.Write("Unesite komandu za upravljanje datotekama i fileovima. Za pomoc unesite ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("help");

            Console.ResetColor();

            string text = "COMMAND MODE";
            int totalDashes = Console.WindowWidth - text.Length - 2;
            int leftDashes = totalDashes / 2;
            int rightDashes = totalDashes - leftDashes;
            string centeredText = new string('-', leftDashes) + " " + text + " " + new string('-', rightDashes);

            while (true)
            {
                Console.WriteLine($"\n{centeredText}" +
                    "\n>");
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

                CheckCommand(command, user, _folderService, _fileService, userFolders, _userService, _sharedItemService, userFiles, _commentService);
                userFolders = _userService.GetFoldersOrFiles<Folder>(user);
            }
        }
        public static void  SharedFilesCommandMode(ISharedItemService sharedItemService, User sharedToUser, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IFileService _fileService, ICommentService _commentService)
        {
            Console.Write("Unesite komandu za upravljanje podijeljenim mapama i datotekama. Za pomoc unesite ");

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
                    HelpMenu.DisplaySharedFilesHelp();
                    continue;
                }

                if (command == "povratak")
                    break;

                CheckSharedFilesCommand(command, sharedItemService, userFolders, userFiles, sharedToUser, _fileService, _commentService);
            }
        }
        private static void CheckSharedFilesCommand(string command, ISharedItemService sharedItemService, IEnumerable<Folder> folders, IEnumerable<Data.Entities.Models.File> files, User sharedToUser, IFileService _fileService, ICommentService _commentService)
        {
            Console.Clear();

            string[] parts = command.Split(" ");
            switch (parts[0])
            {
                case "izbrisi":
                    if (parts[1] == "mapu")
                    {
                        var name = GetName(parts.Skip(2));
                        if (name == null)
                        {
                            Console.WriteLine("ne ispravan oblik komande. Unesite help za pomoc");
                            break;
                        }

                        var folderToRemove = FolderRepository.GetFolder(folders, name);
                        //RemoveFolderAndContentsFromShared(folder, folders, sharedItemService, sharedToUser);

                        var sharedFolderToRemove = sharedItemService.GetSharedItem(folderToRemove.Id, folderToRemove.Owner, sharedToUser, DataType.Folder);

                        var removeFolderStatus = sharedItemService.Remove(sharedFolderToRemove);

                        if (removeFolderStatus == Domain.Enums.Status.Failed)
                        {
                            Console.WriteLine("Pogreska prilikom brisanja foldera iz podijeljenih datoteka");
                            return;
                        }

                        Console.WriteLine($"Uspjesno izbrisan folder: {folderToRemove.Name} iz podijeljenih mapa s vama");

                    }
                    else if (parts[1] == "datoteku")
                    {
                        var name = GetName(parts.Skip(2));
                        if (name == null)
                        {
                            Console.WriteLine("ne ispravan oblik komande. Unesite help za pomoc");
                            break;
                        }

                        var fileToRemove = files.FirstOrDefault(f => f.Name == name);
                        if(fileToRemove == null)
                        {
                            Console.WriteLine("datoteka nije pronadena");
                            return;
                        }

                        var sharedItemToRemove = sharedItemService.GetSharedItem(fileToRemove.Id, fileToRemove.Owner, sharedToUser, DataType.File);

                        var status = sharedItemService.Remove(sharedItemToRemove);
                        if (status != Domain.Enums.Status.Success)
                        {
                            Console.WriteLine($"Pogreska prilikom brisanje datoteke: {name} iz podijeljenih datoteka s vama");
                            return;
                        }

                        Console.WriteLine($"Uspjesno izbrisana datoteka: {name} iz mape podiljenih datoteka s vama");
                    }
                    else
                        Console.WriteLine("ne ispravan oblik komande. Unesite help za pomoc");

                    break;

                case "uredi":
                    if(parts[1] != "datoteku")
                    {
                        Console.WriteLine("Ne ispravna komanda. Unesite help za pomoc");
                        return;
                    }
                    
                    var fileName = GetName(parts.Skip(2));
                    if(fileName == null)
                    {
                        Console.WriteLine("Pogreska prilikom dohvacanja imena");
                        return;
                    }


                    var fileToEdit = files.FirstOrDefault(f => f.Name == fileName);
                    if(fileToEdit == null)
                    {
                        Console.WriteLine($"Datoteka: {fileName} nije pronadena medu datotekama podijeljenima s vama");
                        return;
                    }

                    FileProcessesHelper.ReadAndWriteFileContent(fileToEdit, _fileService);

                    break;

                case "udi":
                    if (parts[1] != "u" || parts[2] != "datoteku" || parts.Length < 4)
                    {
                        Console.WriteLine("Ne ispravan oblik komande. Unesite help za pomoc");
                        return;
                    }

                    var fileNameToShow = GetName(parts.Skip(3));
                    if (fileNameToShow == null)
                    {
                        Console.WriteLine("Pogreska prilikom dohvacanja imena");
                        return;
                    }

                    var file = files.FirstOrDefault(f => f.Name == fileNameToShow);
                    if (file == null)
                    {
                        Console.WriteLine($"Datoteka: {fileNameToShow} nije pronadena medu datotekama podijeljenima s vama");
                        return;
                    }

                    Console.WriteLine($"----------Trenutni sadrzaj datoteke----------\n{file.Content}" +
                        $"\n---------------------------------------------" +
                        $"\nZa prikaz komentara upisite otvori komentare nakon cega mozetet upravljati s njima. Za odustajanje ostavite prazno");

                    while (true)
                    {

                        Console.WriteLine(">");
                        var commentCommand = Console.ReadLine();

                        if (string.IsNullOrEmpty(commentCommand))
                        {
                            Console.WriteLine("Povratak...");
                            return;
                        }

                        if (commentCommand != "otvori komentare")
                        {
                            Console.WriteLine("pogresna komanda. Unesite opet");
                            continue;
                        }

                        ManageComments(sharedToUser, file, sharedItemService, _fileService, _commentService);
                        break;
                    }

                    break;

                default:
                    Console.WriteLine("ne ispravna komanda. Za pomoc unesite help");
                    break;
            }
        }
        private static void ManageComments(User user, File file, ISharedItemService _sharedItemService, IFileService _fileService, ICommentService _commentService)
        {
            CommentAction.ShowComments(file, _commentService);
            while (true)
            {

                Console.WriteLine("Unesite komandu za upravljanje komentarima. Za pommoc unesite help\n >");

                var command = Console.ReadLine();

                if (command == "povratak")
                    return;
                if (command == "help")
                {
                    HelpMenu.DisplayCommentHelp();
                    continue;
                }

                switch (command)
                {
                    case "dodaj komentar":
                        CommentAction.CreateComment(file, user, _commentService);
                        break;

                    case "izbrisi komentar":
                        CommentAction.ShowComments(file, _commentService);
                        CommentAction.DeleteComment(file.Id, _commentService);
                        break;

                    case "uredi komentar":
                        CommentAction.ShowComments(file, _commentService);
                        CommentAction.EditComment(file, _commentService);
                        break;

                    default:
                        Console.WriteLine("Pogresna komanda. Unesite help za pomoc");
                        break;
                }
            }
        }
        private static void CheckCommand(string command, User user, IFolderService _folderService, IFileService _fileService, IEnumerable<Folder> userFolders, IUserService _userService, 
            ISharedItemService _sharedItemService, IEnumerable<File> userFiles, ICommentService _commentService)
        {
            Console.Clear();

            string[] parts = command.Split(" ");
            switch (parts[0])
            {
                case "stvori":
                    CreateFolderFile(parts, user, _folderService, _fileService);
                    break;

                case "udi":
                    ChangeWorkingDirectory(parts, userFolders, _userService, user, _commentService);
                    break;

                case "uredi":
                    EditFile(parts, user, _fileService, _userService);
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

                case "ls":
                    Helper.ShowUserFoldersAndFiles(user, _userService, userFolders, userFiles);
                    ReadInput.WaitForUser();
                    break;

                case "podijeli":
                    StartSharing(parts, userFolders, _folderService, _fileService, _userService, _sharedItemService, user);
                    break;

                case "prestani":
                    StopSharing(parts, userFolders, _folderService, _fileService, _userService, _sharedItemService, user);
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
                CreateItem<Folder>(name, user, _folderService, _fileService);

                ReadInput.WaitForUser();

                return;
            }

            CreateItem<File>(name, user, _folderService, _fileService);
            ReadInput.WaitForUser();
        }
        private static void CreateItem<T>(string name, User user, IFolderService _folderService, IFileService _fileService) where T : class
        {
            bool nameAlreadyExist = false;
            if (typeof(T) == typeof(Folder))
                nameAlreadyExist = _folderService.GetFolderByName(name, user) != null ? true : false;

            else if(typeof(T) == typeof(File))
                nameAlreadyExist = _fileService.GetFileByName(name, user) != null ? true : false;

            while (nameAlreadyExist)
            {
                Console.WriteLine($"{typeof(T).Name} s unesenim imenom vec postoji. Unesite novo ili ostavite prazno za odustat");

                var newName = Console.ReadLine();

                if (string.IsNullOrEmpty(newName))
                {
                    Console.WriteLine("Odustali ste od unosa novog imena. Povratak...");
                    return;
                }

                if(typeof(T) == typeof(Folder))
                    nameAlreadyExist = _folderService.GetFolderByName(newName, user) != null ? true : false;
                else if(typeof(T) == typeof(File))
                    nameAlreadyExist = _fileService.GetFileByName(newName, user) != null ? true : false;
            }

            if (typeof(T) == typeof(Folder))
                Create<Folder>(name, currentFolder, user, _folderService, _fileService);

            else if (typeof(T) == typeof(File))
                Create<File>(name, currentFolder, user, _folderService, _fileService);
            
        }

        private static void ChangeWorkingDirectory(string[] parts, IEnumerable<Folder> userFolders, IUserService _userService, User user, ICommentService _commentService)
        {
            if (parts.Length < 4 || parts[1] != "u" || (parts[2] != "mapu" && parts[2] != "datoteku"))
            {
                Console.WriteLine("Pogresan oblik komande za promjenu trenutne mape. Za pomoc unesite help");
                return;
            }


            var name = GetName(parts.Skip(3));
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Ime ne moze biti prazno");
                return;
            }
            if (parts[2] == "mapu")
            {
                var folder = FolderRepository.GetFolder(userFolders, name);
                if (folder == null)
                {
                    Console.WriteLine("Nije pronaden folder s unesenim imenom");
                    return;
                }

                currentFolder = folder;
                Console.WriteLine($"Trenutno unutar mape: {currentFolder.Name}");
                ReadInput.WaitForUser();

                return;
            }

            var files = _userService.GetFoldersOrFiles<File>(user);
            var file = _userService.GetFoldersOrFiles<File>(user).FirstOrDefault(f => f.Name == name);

            if (file == null)
            {
                Console.WriteLine("Datoteka nije pronadena");
                return;
            }

            CommentAction.CommentCommands(file, user, _commentService);
        }
        private static void EditFile(string[] parts, User user, IFileService _fileService, IUserService _userService)
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

            var file = _userService.GetFoldersOrFiles<File>(user).FirstOrDefault(f => f.Name == name);
            if (file == null)
            {
                Console.WriteLine("Datoteka s unesenim imenom nije pronađena.");
                return;
            }

            FileProcessesHelper.ReadAndWriteFileContent(file, _fileService);

            ReadInput.WaitForUser();
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

                ReadInput.WaitForUser();

                return;
            }


            var fileToDelete = _userService.GetFoldersOrFiles<File>(user).FirstOrDefault(f => f.Name == name);
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

            ReadInput.WaitForUser();
        }

        private static void RenameFolderFile(string[] parts, IEnumerable<Folder> userFolders, IFolderService _folderService, IFileService _fileService, IUserService _userService, User user)
        {
            if (parts.Length < 6 || parts[1] != "naziv" || (parts[2] != "mape" && parts[2] != "datoteke"))
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
                FolderProcessesHelper.RenameFolder(currentName, newName, userFolders, _folderService, user);
                return;
            }

            FileProcessesHelper.RenameFile(currentName, newName, _fileService, _userService, user);
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
            if (userToShare == null)
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

                    ReadInput.WaitForUser();

                    return;
                }
            }

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
        
        public static void StopSharing(string[] parts, IEnumerable<Folder> userFolders, IFolderService _folderService, IFileService _fileService, IUserService _userService, ISharedItemService _sharedItemService, User user)
        {
            if(!Helper.IsValidCommandStopSharing(parts))
            {
                Console.WriteLine("Ne ispravan oblik komande 'Prestani dijeliti'. Za pomoc unesite help");
                return;
            }

            var userToShare = EmailUserValid(parts, _userService);
            if(userToShare == null)
                return;


            if (parts[2] == "mapu")
            {
                HandleStopSharingFolder(user, userToShare, _folderService, _fileService, _userService, _sharedItemService);

                return;
            }
            
            HandleStopSharingFile(user, userToShare, _fileService, _sharedItemService);
            
        }
        private static void HandleStopSharingFolder(User user, User userToShare, IFolderService _folderService, IFileService _fileService, IUserService _userService, ISharedItemService _sharedItemService)
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
                
                if(!ReadInput.ConfirmAction($"Zelite li stvarno prestati dijeliti mapu s korisnikom: {userToShare.Name} "))
                {
                    Console.WriteLine("Odustali ste od akcije");
                    return;
                }

                ProcessFolderAndContents(folder, userFolders, _folderService, _fileService, _userService, user, "prestani dijeliti", _sharedItemService, userToShare);

                ReadInput.WaitForUser();
                return;
            }
        }
        private static void HandleStopSharingFile(User user, User userToShare, IFileService _fileService, ISharedItemService _sharedItemService)
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

                RemoveFileIfShared(file, user, userToShare, _sharedItemService);

                ReadInput.WaitForUser();
                return;
            }
        }
        private static void RemoveFileIfShared(File file, User user, User userToShare, ISharedItemService _sharedItemService)
        {
            if (!_sharedItemService.AlreadyShared(file.Id, userToShare.Id, user.Id, DataType.File))
            {
                Console.WriteLine($"Ova datoteka nije ni podijeljena s korisnikom: {userToShare.Name}");
                return;
            }

            var sharedItem = _sharedItemService.GetSharedItem(file.Id, user, userToShare, DataType.File);

            var status = _sharedItemService.Remove(sharedItem);

            if (status == Domain.Enums.Status.Failed)
            {
                Console.WriteLine($"Pogreska prilikom prestanka dijeljenja datoteke: {file.Name}");
                return;
            }

            if(!ReadInput.ConfirmAction($"zelite li prestati dijeliti datoteku s korisnikom: {userToShare.Name} "))
            {
                Console.WriteLine("odustali ste od akcije");
                return;
            }

            Console.WriteLine($"Uspjesno prekinuto dijeljenje datoteke: {file.Name} s korisnikom: {userToShare.Name + " " + userToShare.Email}");
            ReadInput.WaitForUser();
        }
        private static User? EmailUserValid(string[] parts, IUserService _userService)
        {
            var email = GetName(parts.Skip(3));
            if (email == null)
            {
                Console.WriteLine("Email ne moze biti prazan. Povratak...");
                return null;
            }

            while (true)
            {
                if (!_userService.EmailExists(email))
                {
                    Console.WriteLine("Uneseni email ne postoji. Unesite novi email ili ostavite prazno za odustat");
                    email = Console.ReadLine()?.Trim();

                    if (string.IsNullOrEmpty(email))
                    {
                        Console.WriteLine("Odustali ste od unosa emaila.");
                        return null;
                    }

                    continue;
                }

                var userToShare = _userService.GetUser(email);

                if (userToShare == null)
                {
                    Console.WriteLine("Uneseni korisnik nije pronaden. Unesite novi email ili ostavite prazno za odustat");
                    email = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(email))
                    {
                        Console.WriteLine("Odustali ste od unosa emaila.");
                        return null;
                    }

                    continue;
                }

                return userToShare;
            }
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
        private static void ProcessFolderAndContents(Folder folder, IEnumerable<Folder> allFolders, IFolderService _folderService, IFileService _fileService, IUserService _userService, User user,
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
    }
}