﻿using Drive.Presentation.Interfaces;
using Drive.Presentation.Factories;
using Drive.Data.Entities;
using Drive.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Drive.Domain.Repositories;
using Drive.Data.Entities.Models;
public class Program
{
    public static User? CurrentUser = null;
    public static void Main()
    {

        var options = new DbContextOptionsBuilder<DriveDbContext>()
           .UseNpgsql("Host=127.0.0.1;Port=5432;Database=Drive;User Id=postgres;Password=rootuser")
           .EnableSensitiveDataLogging()
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

        var commentRepository = new CommentRepository(dbContext);
        var commentService = new CommentService(commentRepository);

        MenuFactory.Initialize(userService, folderService, fileService, sharedItemService, commentService);

        IMenu mainMenu = MenuFactory.CreateMenu("MainMenu", null);

        while (true)
        {
            mainMenu.Display();
            mainMenu.HandleInput();
        }
    }
}