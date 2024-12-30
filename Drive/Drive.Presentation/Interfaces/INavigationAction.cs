using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Interfaces
{
    public interface INavigationAction
    {
        string Name { get; }

        void Execute(User user,
                     IEnumerable<Folder> userFolders,
                     IEnumerable<File> userFiles,
                     IUserService userService,
                     IFileService fileService,
                     IFolderService folderService,
                     ICommentService commentService,
                     ISharedItemService sharedItemService);
    }
}
