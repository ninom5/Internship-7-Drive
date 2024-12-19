using Drive.Presentation.Interfaces;
using Drive.Presentation.Factories;
using Drive.Data.Entities;
using Drive.Domain.Services;
using Microsoft.EntityFrameworkCore;
public class Program
{
    public static void Main()
    {
        var options = new DbContextOptionsBuilder<DriveDbContext>()
           .UseNpgsql("Host=127.0.0.1;Port=5432;Database=Drive;User Id=postgres;Password=rootuser")
           .Options;
        var dbContext = new DriveDbContext(options);

        var userService = new UserService(dbContext);

        MenuFactory.Initialize(userService);

        IMenu mainMenu = MenuFactory.CreateMenu("MainMenu");

        while (true)
        {
            mainMenu.Display();
            mainMenu.HandleInput();
        }
    }
}