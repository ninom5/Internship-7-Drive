using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Domain.Repositories;
using Drive.Presentation.Menus.SubMenu;
using Drive.Presentation.Utilities;
using Drive.Data.Enums;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Reader;


namespace Drive.Presentation.Actions
{
    public class CommandSharedAction
    {
        public void SharedFilesCommandMode(ISharedItemService sharedItemService, User sharedToUser, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IFileService _fileService, ICommentService _commentService)
        {
            Console.Clear();

            string heading = "SHARED FILES COMMAND MODE";

            int dashes = Console.WindowWidth - heading.Length - 2;

            int leftDashes = dashes / 2;
            int rightDashes = dashes - leftDashes;

            Console.Write("Unesite komandu za upravljanje podijeljenim mapama i datotekama. Za pomoc unesite ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("help\n");

            Console.ResetColor();

            while (true)
            {
                Console.WriteLine(new string('-', leftDashes) + heading + new string('-', rightDashes));

                Console.WriteLine("\n>");
                var command = Console.ReadLine()?.Trim();

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
            string[] parts = command.Split(" ");
            switch (parts[0])
            {
                case "izbrisi":
                    Console.Clear();

                    if (parts[1] == "mapu")
                        DeleteSharedFolder(parts, folders, sharedItemService, sharedToUser);
                    else if (parts[1] == "datoteku")
                        DeleteSharedFile(parts, files, sharedItemService, sharedToUser, _commentService);
                    else
                        Console.WriteLine("ne ispravan oblik komande. Unesite help za pomoc");

                    break;

                case "uredi":
                    Console.Clear();

                    if (parts[1] != "datoteku")
                    {
                        Console.WriteLine("Ne ispravna komanda. Unesite help za pomoc");
                        return;
                    }

                    var fileName = Helper.GetName(parts.Skip(2));
                    if (fileName == null)
                    {
                        Console.WriteLine("Pogreska prilikom dohvacanja imena");
                        return;
                    }


                    var fileToEdit = files.FirstOrDefault(f => f.Name == fileName);
                    if (fileToEdit == null)
                    {
                        Console.WriteLine($"Datoteka: {fileName} nije pronadena medu datotekama podijeljenima s vama");
                        return;
                    }

                    FileProcessesHelper.ReadAndWriteFileContent(fileToEdit, _fileService);

                    break;

                case "udi":
                    Console.Clear();

                    if (parts[1] != "u" || parts[2] != "datoteku" || parts.Length < 4)
                    {
                        Console.WriteLine("Ne ispravan oblik komande. Unesite help za pomoc");
                        return;
                    }

                    var fileNameToShow = Helper.GetName(parts.Skip(3));
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

                    string text = "Trenutni sadrzaj datoteke";
                    int totalDashes = Console.WindowWidth - text.Length - 2;

                    int leftDashes = totalDashes / 2;
                    int rightDashes = totalDashes - leftDashes;

                    string centeredText = new string('-', leftDashes) + " " + text + " " + new string('-', rightDashes);

                    Console.WriteLine($"{centeredText}\n{file.Content}" +
                        $"\n{new string('-', totalDashes + text.Length)}" +
                        $"\nZa prikaz komentara upisite otvori komentare nakon cega mozetet upravljati s njima. Za odustajanje ostavite prazno");

                    while (true)
                    {
                         
                        Console.WriteLine(">");
                        var commentCommand = Console.ReadLine()?.Trim();

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
                    Console.Clear();
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

                var command = Console.ReadLine()?.Trim();

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
                        Console.Clear();
                        CommentAction.CreateComment(file, user, _commentService);
                        break;

                    case "izbrisi komentar":
                        Console.Clear();
                        CommentAction.ShowComments(file, _commentService);
                        CommentAction.DeleteComment(file.Id, _commentService, user);
                        break;

                    case "uredi komentar":
                        Console.Clear();
                        CommentAction.ShowComments(file, _commentService);
                        CommentAction.EditComment(file, _commentService, user);
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Pogresna komanda. Unesite help za pomoc");
                        break;
                }
            }
        }
        private static void DeleteSharedFolder(string[] parts, IEnumerable<Folder> folders, ISharedItemService sharedItemService, User sharedToUser)
        {
            var name = Helper.GetName(parts.Skip(2));
            if (name == null)
            {
                Console.WriteLine("ne ispravan oblik komande. Unesite help za pomoc");
                return;
            }

            var folderToRemove = FolderRepository.GetFolder(folders, name);

            var sharedFolderToRemove = sharedItemService.GetSharedItem(folderToRemove.Id, folderToRemove.Owner, sharedToUser, DataType.Folder);

            var removeFolderStatus = sharedItemService.Remove(sharedFolderToRemove);

            if (removeFolderStatus == Domain.Enums.Status.Failed)
            {
                Console.WriteLine("Pogreska prilikom brisanja foldera iz podijeljenih datoteka");
                return;
            }

            Console.WriteLine($"Uspjesno izbrisan folder: {folderToRemove.Name} iz podijeljenih mapa s vama");
        }
        private static void DeleteSharedFile(string[] parts, IEnumerable<File> files, ISharedItemService sharedItemService, User sharedToUser, ICommentService _commentService)
        {
            var name = Helper.GetName(parts.Skip(2));
            if (name == null)
            {
                Console.WriteLine("ne ispravan oblik komande. Unesite help za pomoc");
                return;
            }

            var fileToRemove = files.FirstOrDefault(f => f.Name == name);
            if (fileToRemove == null)
            {
                Console.WriteLine("datoteka nije pronadena");
                return;
            }

            var sharedItemToRemove = sharedItemService.GetSharedItem(fileToRemove.Id, fileToRemove.Owner, sharedToUser, DataType.File);

            if(!ReadInput.ConfirmAction("Zelite li stvarno izbrisati datoteku iz podijeljenih datoteka s vama "))
            {
                Console.WriteLine("odustali ste od brisanja");
                return;
            }

            var listOfUserComments = _commentService.GetCommentsByFile(fileToRemove).Where(item => item.UserId == sharedToUser.Id);
            foreach (var comment in listOfUserComments)
            {
                if(_commentService.RemoveComment(comment) == Domain.Enums.Status.Failed)
                    Console.WriteLine($"Pogreska prilikom brisanja komentara: {comment.Id} vasih komentara za odabrani file");
                else
                    Console.WriteLine($"uspjesno brisanja komentara: {comment.Id} za odabrani file");
            }

            var status = sharedItemService.Remove(sharedItemToRemove);
            if (status != Domain.Enums.Status.Success)
            {
                Console.WriteLine($"\nPogreska prilikom brisanje datoteke: {name} iz podijeljenih datoteka s vama");
                return;
            }

            Console.WriteLine($"\nUspjesno izbrisana datoteka: {name} iz mape podiljenih datoteka s vama");
        }
    }
}
