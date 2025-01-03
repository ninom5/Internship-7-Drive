using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Actions.Navigation
{
    public class NavigationInput
    {
        public static void NavigationMode(User user, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IUserService _userService, IFileService _fileService, IFolderService _folderService, ICommentService _commentService,
           ISharedItemService _sharedItemService)
        {
            Console.Clear();
            int selectedIndex = 0;

            var actionFactory = new NavigationActionFactory();
            var actions = actionFactory.GetActionNames();

            while (true)
            {
                Console.Clear();

                string text = "NAVIGATION MODE";

                int totalDashes = Console.WindowWidth - text.Length - 2;
                int leftDashes = totalDashes / 2;
                int rightDashes = totalDashes - leftDashes;

                string centeredText = new string('-', leftDashes) + " " + text + " " + new string('-', rightDashes);

                Console.ForegroundColor = ConsoleColor.DarkGreen;

                Console.WriteLine($"\n{centeredText}\n");

                Console.ResetColor();

                for (int i = 0; i < actions.Count; i++)
                {
                    if (i == selectedIndex)
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                    else
                        Console.BackgroundColor = ConsoleColor.Black;

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(actions[i]);
                    Console.ResetColor();
                }

                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.UpArrow)
                    selectedIndex = selectedIndex == 0 ? actions.Count - 1 : selectedIndex - 1;

                else if (key.Key == ConsoleKey.DownArrow)
                    selectedIndex = selectedIndex == actions.Count - 1 ? 0 : selectedIndex + 1;

                else if (key.Key == ConsoleKey.Enter)
                {
                    var selectedActionName = actions[selectedIndex];
                    var selectedAction = actionFactory.GetAction(selectedActionName);

                    if (selectedAction != null)
                    {
                        Console.Clear();
                        selectedAction.Execute(user, userFolders, userFiles, _userService, _fileService, _folderService, _commentService, _sharedItemService);
                    }
                    else
                    {
                        Console.WriteLine("Problem s odabirom akcije. Pokusajte ponovo");
                    }
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    break;
                }
            }
        }
    }
}
