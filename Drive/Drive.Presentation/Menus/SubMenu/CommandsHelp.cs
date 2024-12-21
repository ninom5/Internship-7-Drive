using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces;
using Drive.Presentation.Factories;
using Drive.Presentation.Interfaces;

namespace Drive.Presentation.Menus.SubMenu
{
    public class CommandsHelp : BaseMenu
    {
        private readonly IUserService userService;
        private readonly User user;

        public CommandsHelp() : base("HelpMenu")
        {
            //Options.Add(("stvori mapu 'ime mape'", null));
            //Options.Add(("stvori datoteku ‘ime datoteke’", null));
            //Options.Add(("uđi u mapu ‘ime mape’", null));
            //Options.Add(("uredi datoteku ‘ime datoteke’", null));
            //Options.Add(("izbriši mapu/datoteku ‘ime mape/datoteke’", null));
            //Options.Add(("promjeni naziv mape/datoteke ‘ime mape/datoteke’ u ‘novo ime mape/datoteke’", null));
        }
        public void Execute()
        {
            IMenu commandsShow = MenuFactory.CreateLoginMenu(user);
            while(true)
            {
                commandsShow.Display();
            }
        }
    }
}
