using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Actions.Command;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Reader;
using Drive.Presentation.Utilities;


namespace Drive.Presentation.Actions.Disk
{
    public class UserSharedFilesAction : IAction
    {
        private readonly IUserService _userService;
        private readonly ISharedItemService _sharedItemService;
        private readonly IFileService _fileService;
        private readonly ICommentService _commentService;
        private readonly User _LoggedUser;

        public UserSharedFilesAction(IUserService userService, User user, ISharedItemService sharedItemService, IFileService fileService, ICommentService commentService)
        {
            _userService = userService;
            _sharedItemService = sharedItemService;
            _LoggedUser = user;
            _fileService = fileService;
            _commentService = commentService;
        }

        public void Execute()
        {
            Console.Clear();

            IEnumerable<Folder> folders;
            IEnumerable<Data.Entities.Models.File> files;

            (folders, files) = Helper.ShowSharedDataWithUser(_sharedItemService, _LoggedUser);

            Console.WriteLine();
            ReadInput.WaitForUser();


            if (!folders.Any() && !files.Any())
            {
                Console.WriteLine("nema podijeljenih mapa i datoteka s vama, pa ne mozete upravljati. Povratak na user menu...");
                Thread.Sleep(1000);

                return;
            }

            CommandSharedAction commandSharedAction = new CommandSharedAction();
            commandSharedAction.SharedFilesCommandMode(_sharedItemService, _LoggedUser, folders, files, _fileService, _commentService);
        }
    }
}
