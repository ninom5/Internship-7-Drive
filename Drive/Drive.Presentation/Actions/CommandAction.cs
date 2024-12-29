using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Domain.Repositories;
using Drive.Presentation.Menus.SubMenu;
using Drive.Presentation.Reader;
using Drive.Presentation.Utilities;
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
                var command = Console.ReadLine()?.Trim();

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
                userFiles = _userService.GetFoldersOrFiles<File>(user);
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
                    DeleteFolderFile(parts, user, _folderService, _fileService, _userService, userFolders, _commentService, _sharedItemService);
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
                    StartSharing(parts, userFolders, _folderService, _fileService, _userService, _sharedItemService, user, _commentService);
                    break;

                case "prestani":
                    StopSharing(parts, userFolders, _folderService, _fileService, _userService, _sharedItemService, user, _commentService);
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
            string name = Helper.GetName(parts.Skip(2));

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
                Helper.Create<Folder>(name, currentFolder, user, _folderService, _fileService);

            else if (typeof(T) == typeof(File))
                Helper.Create<File>(name, currentFolder, user, _folderService, _fileService);
            
        }

        private static void ChangeWorkingDirectory(string[] parts, IEnumerable<Folder> userFolders, IUserService _userService, User user, ICommentService _commentService)
        {
            if (parts.Length < 4 || parts[1] != "u" || (parts[2] != "mapu" && parts[2] != "datoteku"))
            {
                Console.WriteLine("Pogresan oblik komande za promjenu trenutne mape. Za pomoc unesite help");
                return;
            }


            var name = Helper.GetName(parts.Skip(3));
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

            var name = Helper.GetName(parts.Skip(2));
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
        private static void DeleteFolderFile(string[] parts, User user, IFolderService _folderService, IFileService _fileService, IUserService _userService, IEnumerable<Folder> userFolders, 
            ICommentService _commentService, ISharedItemService _sharedItemService)
        {
            if (parts.Length < 3 || (parts[1] != "mapu" && parts[1] != "datoteku"))
            {
                Console.WriteLine("Pogresan oblik komande za promjenu trenutne mape. Za pomoc unesite help");
                return;
            }

            string dataType = parts[1];
            string name = Helper.GetName(parts.Skip(2));

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

                Helper.ProcessFolderAndContents(folderToDelete, userFolders, _folderService, _fileService, _userService, user, "izbrisi", null, null);

                ReadInput.WaitForUser();

                return;
            }


            var fileToDelete = _userService.GetFoldersOrFiles<File>(user).FirstOrDefault(f => f.Name == name);
            if (fileToDelete == null)
            {
                Console.WriteLine("Nije pronaden zeljeni file");
                return;
            }

            if (!ReadInput.ConfirmAction("Zelite li stvarno izbrisati datoteku iz podijeljenih datoteka s vama "))
            {
                Console.WriteLine("odustali ste od brisanja");
                return;
            }

            var listOfSharedItems = _sharedItemService.GetAllUserShared(user).Where(item => item.ItemType == Data.Enums.DataType.File && item.Id == fileToDelete.Id);

            foreach (var sharedItem in listOfSharedItems)
            {
                SharedItemsProcesses.RemoveFileIfShared(fileToDelete, user, sharedItem.SharedWith, _sharedItemService, _commentService);
                //if (_sharedItemService.Remove(sharedItem) == Domain.Enums.Status.Failed)
                //    Console.WriteLine($"Pogreska prilikom brisanja podijeljene datoteke: {sharedItem.Id} s korisnikom: {sharedItem.SharedWith}");
                //else
                //    Console.WriteLine($"uspjesno brisanja podijeljene datoteke: {sharedItem.Id} s korisnikom: {sharedItem.SharedWith.Name}");
            }

            var deleteFileStatus = _fileService.DeleteFile(fileToDelete);
            if (deleteFileStatus != Domain.Enums.Status.Success)
            {
                Console.WriteLine($"\npogreska prilikom brisanja filea: {fileToDelete.Name}");
                return;
            }

            Console.WriteLine($"\nDatoteka {fileToDelete.Name} s id: {fileToDelete.Id} uspjesno obrisana");

            ReadInput.WaitForUser();
        }

        private static void RenameFolderFile(string[] parts, IEnumerable<Folder> userFolders, IFolderService _folderService, IFileService _fileService, IUserService _userService, User user)
        {
            var names = Helper.CheckRenameCommand(parts);

            if (string.IsNullOrEmpty(names.Item1) || string.IsNullOrEmpty(names.Item2))
                return;

            string currentName = names.Item1;
            string newName = names.Item2;

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

        private static void StartSharing(string[] parts, IEnumerable<Folder> userFolders, IFolderService _folderService, IFileService _fileService, IUserService _userService, ISharedItemService _sharedItemService, User user,
            ICommentService _commentService)
        {
            var userToShare = Helper.ValidStartShareingCommandAndUser(parts, _userService, user);
            if (userToShare == null)
                return;


            if (parts[1] == "mapu")
            {   
                SharedItemsProcesses.StartSharingFolder(user, userToShare, _folderService, _sharedItemService, _userService, _fileService, userFolders, _commentService);
                return;
            }

            SharedItemsProcesses.StartSharingFile(user, userToShare, _sharedItemService, _userService);
        }
        
        public static void StopSharing(string[] parts, IEnumerable<Folder> userFolders, IFolderService _folderService, IFileService _fileService, IUserService _userService, 
            ISharedItemService _sharedItemService, User user, ICommentService _commentService)
        {
            if(!Helper.IsValidCommandStopSharing(parts))
            {
                Console.WriteLine("Ne ispravan oblik komande 'Prestani dijeliti'. Za pomoc unesite help");
                return;
            }

            var userToShare = Helper.EmailUserValid(parts, _userService);
            if(userToShare == null)
                return;


            if (parts[2] == "mapu")
            {
                SharedItemsProcesses.HandleStopSharingFolder(user, userToShare, _folderService, _fileService, _userService, _sharedItemService, _commentService);
                return;
            }

            SharedItemsProcesses.HandleStopSharingFile(user, userToShare, _fileService, _sharedItemService, _commentService);
            
        }
        
    }
}