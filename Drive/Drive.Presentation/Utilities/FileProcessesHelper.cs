using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Data.Enums;
using Drive.Domain.Services;
using Drive.Presentation.Reader;

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
                Console.WriteLine($"Datoteka: {file.Name} nije ni podijeljena s korisnikom: {shareToUser.Name}");
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
        public static void ReadAndWriteFileContent(Drive.Data.Entities.Models.File file, IFileService _fileService)
        {
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
                        else if (currentLine == "help")
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
                else if (key.Key == ConsoleKey.Z && key.Modifiers == ConsoleModifiers.Control)
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
                ReadInput.WaitForUser();
            }
        }
    }
}
