using Drive.Domain.Interfaces;
using Drive.Presentation.Interfaces;
using Drive.Presentation.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
