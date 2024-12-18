using Drive.Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.Presentation.Actions
{
    public class LoginAction : IAction
    {
        public void Execute()
        {
            Console.WriteLine("Otvorena forma za prijavu...");
            Console.ReadKey();
        }
    }
}
