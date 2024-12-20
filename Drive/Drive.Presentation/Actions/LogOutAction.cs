using Drive.Domain.Interfaces;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Menus;

namespace Drive.Presentation.Actions
{
    public class LogOutAction : IAction
    {   
        private readonly IUserService _userService;
        public LogOutAction(IUserService userService)
        { 
            _userService = userService;
        }
        public void Execute()
        {
            Console.WriteLine("Odjava. Povratak na glavni izbornik...");
            var mainMenu = new MainMenu(_userService);
            mainMenu.Display();
            mainMenu.HandleInput();
        }
    }
}
