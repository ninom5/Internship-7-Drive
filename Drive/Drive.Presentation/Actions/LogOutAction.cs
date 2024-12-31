using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Factories;
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
            Console.Clear();

            Console.WriteLine("Odjava...");

            Program.CurrentUser = null;

            var mainMenu = MenuFactory.CreateMenu("MainMenu", null);
            mainMenu.Display();
            mainMenu.HandleInput();
        }
    }
}
