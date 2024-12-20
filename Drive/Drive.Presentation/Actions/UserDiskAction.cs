﻿using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces;
using Drive.Presentation.Interfaces;
using System.Collections.Immutable;

namespace Drive.Presentation.Actions
{
    public class UserDiskAction : IAction
    {
        private readonly IUserService _userService;
        private readonly User _user;

        public UserDiskAction(IUserService userService, User user)
        {
            _userService = userService;
            _user = user;
        }

        public void Execute()
        {
            var userFolders = _userService.GetFoldersOrFiles<Folder>(_user);
            var userFiles = _userService.GetFoldersOrFiles<Drive.Data.Entities.Models.File>(_user);

            var userFoldersSorted = userFolders.OrderBy(folder => folder.Name);

            foreach (var folder in userFoldersSorted)
            {
                Console.WriteLine($"Mapa: {folder.Name}, Id mape: {folder.Id}");

                var folderFiles = userFiles.Where(file => file.FolderId == folder.Id).OrderBy(file => file.LastModifiedAt);

                foreach(var file in folderFiles)
                {
                    Console.WriteLine($"\tFile: {file.Name}, mapa id kojoj pripada{file.FolderId}");
                }
            }

            Console.Write("Unesite komandu za upravljanje datotekama i fileovima. Za pomoc unesite ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("help");

            Console.ResetColor();

            Console.ReadKey();
        }
    }
}
