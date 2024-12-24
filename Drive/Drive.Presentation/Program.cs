using Drive.Presentation.Interfaces;
using Drive.Presentation.Factories;
using Drive.Data.Entities;
using Drive.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Drive.Domain.Repositories;
public class Program
{
    public static void Main()
    {
        Console.Title = "DUMP Drive";

        var options = new DbContextOptionsBuilder<DriveDbContext>()
           .UseNpgsql("Host=127.0.0.1;Port=5432;Database=Drive;User Id=postgres;Password=rootuser")
           .Options;
        var dbContext = new DriveDbContext(options);

        var userRepository = new UserRepository(dbContext);

        var userService = new UserService(userRepository);

        var folderRepository = new FolderRepository(dbContext);
        var folderService = new FolderService(folderRepository);

        var fileRepository = new FileRepository(dbContext);
        var fileService = new FileService(fileRepository);

        var sharedItemRepository = new SharedItemRepository(dbContext);
        var sharedItemService = new SharedItemsService(sharedItemRepository);

        MenuFactory.Initialize(userService, folderService, fileService, sharedItemService);

        IMenu mainMenu = MenuFactory.CreateMenu("MainMenu");

        while (true)
        {
            mainMenu.Display();
            mainMenu.HandleInput();
        }
    }
}