using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces;
using Drive.Domain.Repositories;
using Drive.Presentation.Menus.SubMenu;
using System.Text;
using System.Text.RegularExpressions;


namespace Drive.Presentation.Actions
{
    public class CommandAction
    {
        public static Folder currentFolder = null;
        public static void CommandMode(User user, IFolderService _folderService, Folder parrentFolder, IFileService _fileService, IEnumerable<Folder> userFolders, IUserService _userService)
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

                CheckCommand(command, user, _folderService, _fileService, userFolders, _userService);
                userFolders = _userService.GetFoldersOrFiles<Folder>(user);
            }
        }

        private static void CheckCommand(string command, User user, IFolderService _folderService, IFileService _fileService, IEnumerable<Folder> userFolders, IUserService _userService)
        {
            Console.Clear();

            string[] parts = command.Split(" ");
            switch (parts[0])
            {
                case "stvori":
                    if (parts[1] == "mapu")
                    {
                        string pattern = @"'([^']*)'";

                        Match match = Regex.Match(parts[2], pattern);

                        Create<Folder>(match.Groups[1].Value, currentFolder, user, _folderService, _fileService);
                    }
                    else if(parts[1] == "datoteku")
                    {
                        string pattern = @"'([^']*)'";

                        Match match = Regex.Match(parts[2], pattern);

                        Create<Drive.Data.Entities.Models.File>(match.Groups[1].Value, currentFolder, user, _folderService, _fileService);
                    }
                    else
                        Console.WriteLine("pogresna komanda. Za pomoc unesite help");

                    break;
                case "udi":
                    if(parts.Length > 3 && parts[1] == "u" && parts[2] == "mapu")
                    {
                        string pattern = @"'([^']*)'";

                        Match match = Regex.Match(parts[3], pattern);

                        if (!match.Success)
                        {
                            Console.WriteLine("pogreska");
                            return;
                        }

                        currentFolder = FolderRepository.GetFolder(userFolders, match.Groups[1].Value);

                        if(currentFolder == null)
                        { 
                            Console.WriteLine("Mapa nije pronadena"); 
                            return;
                        }

                        Console.WriteLine($"Trenutno unutar mape: {currentFolder.Name}");
                    }
                    else
                        Console.WriteLine("Pogresna komanda. Za pomoc unesite help");
                    break;
                case "izbrisi":
                    if (parts[1] == "mapu")
                    {
                        string pattern = @"'([^']*)'";

                        Match match = Regex.Match(parts[2], pattern);
                        if(!match.Success)
                        {
                            Console.WriteLine("ne ispravan unos");
                            return;
                        }


                        var folderToDelete = FolderRepository.GetFolder(userFolders, match.Groups[1].Value);
                        if(folderToDelete == null)
                        {
                            Console.WriteLine("Nije pronaden zelejni folder");
                            return;
                        }

                        DeleteFolderAndContents(folderToDelete, userFolders, _folderService, _fileService, _userService, user);

                        Console.WriteLine($"Folder: {folderToDelete.Name} s id: {folderToDelete.Id} uspjesno izbrisan");

                    }
                    else if (parts[1] == "datoteku") //prominit na file sad je na folder!! i brisanje svega unutar tog foldera !! dodat nemogucnost brisanja root foldera!!
                    {
                        string pattern = @"'([^']*)'";

                        Match match = Regex.Match(parts[2], pattern);

                        var fileToDelete = FolderRepository.GetFolder(userFolders, match.Groups[1].Value);
                        if (fileToDelete == null)
                        {
                            Console.WriteLine("Nije pronaden zelejni folder");
                            return;
                        }

                        var deletingStatus = _folderService.DeleteFolder(fileToDelete);
                        if (deletingStatus != Domain.Enums.Status.Success)
                        {
                            Console.WriteLine("pogreska prilikom brisanja foldera");
                            return;
                        }

                        Console.WriteLine($"File: {fileToDelete.Name} s id: {fileToDelete.Id} uspjesno izbrisan");
                    }
                    else
                        Console.WriteLine("pogresna komanda. Za pomoc unesite help");
                    break;
                case "trenutni_direktorij":
                    Console.WriteLine($"Trenutno se nalazite u mapi: {currentFolder.Name}");
                    break;
                default:
                    Console.WriteLine("Pogresna komanda unesite opet");
                    break;
            }
        }
        public static void Create<T>(string name, Folder folder, User user, IFolderService ?_folderService, IFileService ?_fileService)
        {
            if (typeof(T) == typeof(Folder))
            {
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

                while(true)
                {
                    var lineOfContent = Console.ReadLine();
                    if (string.IsNullOrEmpty(lineOfContent))
                        break;

                    stringBuilder.AppendLine(lineOfContent);
                }

                var content = stringBuilder.ToString();
                if(string.IsNullOrEmpty(content))
                {
                    Console.WriteLine("Sadrzaj ne moze biti prazan. Povratak...");
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
        private static void DeleteFolderAndContents(Folder folderToDelete, IEnumerable<Folder> allFolders, IFolderService _folderService, IFileService _fileService, IUserService _userService, User user)
        {
            var subFolders = allFolders.Where(f => f.ParentFolderId == folderToDelete.Id).ToList();

            foreach (var subFolder in subFolders)
                DeleteFolderAndContents(subFolder, allFolders, _folderService, _fileService, _userService, user);
            
            var filesInFolder = _userService.GetFoldersOrFiles<Drive.Data.Entities.Models.File>(user)
                                           .Where(file => file.FolderId == folderToDelete.Id)
                                           .ToList();

            //foreach (var file in filesInFolder)
            //{
            //    var deleteFileStatus = _fileService.DeleteFile(file);
            //    if (deleteFileStatus == Domain.Enums.Status.Success)
            //    {
            //        Console.WriteLine($"Uspješno izbrisana datoteka: {file.Name} u mapi: {folderToDelete.Name}");
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Pogreška prilikom brisanja datoteke: {file.Name}");
            //    }
            //}

            var deleteFolderStatus = _folderService.DeleteFolder(folderToDelete);
            if (deleteFolderStatus != Domain.Enums.Status.Success)
            {
                Console.WriteLine($"Pogreška prilikom brisanja mape: {folderToDelete.Name}");
            }
                
            Console.WriteLine($"Uspješno izbrisana mapa: {folderToDelete.Name}");
        }

    }
}