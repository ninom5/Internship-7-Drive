﻿

using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces;
using Drive.Domain.Services;


namespace Drive.Presentation.Actions
{
    public class CommandAction
    {  
        public static void CommandMode(User user, IFolderService _folderService, Folder parrentFolder)
        {
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

                if (command == "povratak")
                    break;

                CheckCommand(command, parrentFolder, user, _folderService);
            }
        }

        private static void CheckCommand(string command, Folder parrentFolder, User user, IFolderService _folderService)
        {
            string[] parts = command.Split(" ");
            switch (parts[0])
            {
                case "stvori":
                    if (parts[1] == "mapu")
                    {
                        Create<Folder>(parts[2], parrentFolder, user, _folderService);
                    }
                    
                    return;
            }
        }
        public static void Create<T>(string name, Folder parrentFolder, User user, IFolderService _folderService)
        { 
            if(typeof(T) == typeof(Folder))
            {
                var creatingFolderStatus = _folderService.CreateFolder(name, user, parrentFolder);
            }
        }
    }
}