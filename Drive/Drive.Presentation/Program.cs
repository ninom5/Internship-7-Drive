using Drive.Presentation.Interfaces;
using Drive.Presentation.Factories;
public class Program
{
    public static void Main()
    {
        IMenu mainMenu = MenuFactory.CreateMenu("MainMenu");

        while(true)
        {
            mainMenu.Display();
            mainMenu.HandleInput();
        }
    }
}