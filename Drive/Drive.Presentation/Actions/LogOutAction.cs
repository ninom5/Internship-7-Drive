using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Interfaces;

namespace Drive.Presentation.Actions
{
    public class LogOutAction : IAction
    {   
        private readonly IUserService _userService;
        private readonly IFolderService _folderService;
        public LogOutAction(IUserService userService, IFolderService folderService)
        {
            _userService = userService;
            _folderService = folderService;
        }
        public void Execute()
        {
            //Console.WriteLine("Odjava. Povratak na glavni izbornik...");
            ////var mainMenu = new MainMenu(_userService, _folderService);
            //mainMenu.Display();
            //mainMenu.HandleInput();
        }
    }
}
