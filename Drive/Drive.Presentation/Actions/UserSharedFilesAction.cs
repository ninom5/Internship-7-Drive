using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Domain.Services;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Reader;
using System.Collections.Generic;

namespace Drive.Presentation.Actions
{
    public class UserSharedFilesAction : IAction
    {
        private readonly IUserService _userService;
        private readonly ISharedItemService _sharedItemService;
        private readonly User _LoggedUser;

        public UserSharedFilesAction(IUserService userService, User user, ISharedItemService sharedItemService)
        {
            _userService = userService;
            _sharedItemService = sharedItemService;
            _LoggedUser = user;
        }

        public void Execute()
        {
            Console.Clear();

            var folders = _sharedItemService.GetAllSharedWithUser(_LoggedUser, Data.Enums.DataType.Folder).OrderBy(f => f.Folder.Name);
            var files = _sharedItemService.GetAllSharedWithUser(_LoggedUser, Data.Enums.DataType.File).OrderBy(f => f.File.LastModifiedAt);

            Console.WriteLine(" Podijeljene mape s vama: ");
            foreach (var folder in folders)
            {
                if (folder != null)
                {
                    Console.WriteLine($"\t-Mapa: {folder.Folder.Name} id mape: {folder.Folder.Id} podijeljena od korisnika: {folder.Folder.Owner.Name}");
                }
                else
                {
                    Console.WriteLine("\tNema mapa podijeljenih s vama");
                    break;
                }
            }

            Console.WriteLine("\n Podijeljene datoteke s vama: ");

            foreach (var file in files)
            {
                if (file != null)
                {
                    Console.WriteLine($"\t-Datoteka: {file.File.Name} podijeljena od korisnika: {file.File.Owner.Name} unutar mape: {file.File.Folder.Name}, zadnji put promijenjena: {file.File.LastModifiedAt}");
                }
                else
                {
                    Console.WriteLine("\tNema datoteka podijeljenih s vama");
                }
            }
            Console.WriteLine();
            ReadInput.WaitForUser();

            //CommandAction.SharedFilesCommandMode(_sharedItemService, _LoggedUser, folders, files);
        }
    }
}
