using Drive.Presentation.Interfaces;


namespace Drive.Presentation.Menus
{
    public abstract class BaseMenu : IMenu
    {
        protected string Title;
        protected List<(string OptionText, IAction Action)> Options;

        public BaseMenu(string title) 
        {
            Title = title;
            Options = new List<(string, IAction)>();
        }

        public virtual void Display()
        {
            Console.Clear();
            Console.WriteLine($"------------{Title}----------");

            for (int i = 0; i < Options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Options[i].OptionText}");
            }
        }

        public virtual void HandleInput()
        {
            int choice = Reader.ReadInput.ReadNumberChoice("Odaberite akciju: ", 1, Options.Count);

            Options[choice - 1].Action.Execute();
        }
    }
}
