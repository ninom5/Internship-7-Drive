using Drive.Presentation.Interfaces;
using Drive.Presentation.Factories;
using Drive.Domain.Factories;
using Drive.Data.Entities.Models;
public class Program
{
    public static User? CurrentUser = null;
    public static void Main()
    {

        var (userService, folderService, fileService, sharedItemsService, commentService) = DbContext.CreateServices();

        MenuFactory.Initialize(userService, folderService, fileService, sharedItemsService, commentService);

        IMenu mainMenu = MenuFactory.CreateMenu("MainMenu", null);

        while (true)
        {
            mainMenu.Display();
            mainMenu.HandleInput();
        }
    }
}