using Drive.Presentation.Interfaces;

namespace Drive.Presentation.Actions
{
    public class ExitAction : IAction
    {
        public void Execute() 
        {
            Console.WriteLine("Exiting...");
            Environment.Exit(0);
        }
    }
}
