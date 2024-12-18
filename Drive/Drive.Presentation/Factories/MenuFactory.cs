using Drive.Presentation.Interfaces;
using Drive.Presentation.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.Presentation.Factories
{
    public static class MenuFactory
    {
        public static IMenu CreateMenu(string menuType)
        {
            return menuType switch
            {
                "Main Menu" => new MainMenu(),
            };
        }
    }
}
