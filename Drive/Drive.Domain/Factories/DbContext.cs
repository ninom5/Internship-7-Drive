using Drive.Data.Entities;
using Drive.Domain.Interfaces.Services;
using Drive.Domain.Repositories;
using Drive.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Drive.Domain.Factories
{
    public class DbContext
    {
        public static DbContextOptions<DriveDbContext> CreateDbContextOptions()
        {
            return new DbContextOptionsBuilder<DriveDbContext>()
                .UseNpgsql("Host=127.0.0.1;Port=5432;Database=Drive;User Id=postgres;Password=rootuser")
                .EnableSensitiveDataLogging()
                .Options;
        }

        public static DriveDbContext CreateDbContext()
        {
            var options = CreateDbContextOptions();
            return new DriveDbContext(options);
        }

        public static (IUserService, IFolderService, IFileService, ISharedItemService, ICommentService) CreateServices()
        {
            var dbContext = CreateDbContext();

            var userService = new UserService(new UserRepository(dbContext));
            var folderService = new FolderService(new FolderRepository(dbContext));
            var fileService = new FileService(new FileRepository(dbContext));
            var sharedItemsService = new SharedItemsService(new SharedItemRepository(dbContext));
            var commentService = new CommentService(new CommentRepository(dbContext));

            return (userService, folderService, fileService, sharedItemsService, commentService);
        }
    }
}