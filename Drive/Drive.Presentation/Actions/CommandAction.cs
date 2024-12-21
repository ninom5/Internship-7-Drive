

using Drive.Data.Entities.Models;
using Drive.Domain.Services;
using System.Reflection.Metadata.Ecma335;

namespace Drive.Presentation.Actions
{
    public class CommandAction
    {  
        public static void CommandMode(User user, Folder folder)
        {
            Console.Write("Unesite komandu za upravljanje datotekama i fileovima. Za pomoc unesite ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("help");

            Console.ResetColor();

            int currentFolder = folder.Id;

            while (true)
            {
                Console.WriteLine("\n>");
                var command = Console.ReadLine();

                if (string.IsNullOrEmpty(command))
                    continue;

                if (command == "povratak")
                    break;

                CheckCommand(command, currentFolder);
            }
        }

        private static void CheckCommand(string command, int currentFolder)
        {
            string[] parts = command.Split(" ");
            switch (parts[0])
            {
                case "stvori":
                    if (parts[1] == "mapu")
                    {
                        Create<Folder>(parts[2], currentFolder);
                    }
                    
                    return;
            }
        }
        public static void Create<T>(string name, int currentFolder)
        { 
            if(typeof(T) == typeof(Folder))
            {
                //FolderService folderService = new FolderService();
            }
        }
    }
}