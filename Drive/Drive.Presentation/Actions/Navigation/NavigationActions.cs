using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Reader;
using Drive.Presentation.Utilities;
using Drive.Presentation.Interfaces;
using File = Drive.Data.Entities.Models.File;
using Drive.Presentation.Actions;

public class NavigationActions : INavigationAction
{
    public string Name => "stvori datoteku";

    public void Execute(User user, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IUserService userService, IFileService fileService, IFolderService folderService,
        ICommentService commentService, ISharedItemService sharedItemService)
    {
        Console.Clear();
        var fileName = Helper.GetName("unesite naziv datoteke koju zelite stvoriti");
        if (fileName == null) return;

        CommandAction.CreateItem<File>(fileName, user, folderService, fileService);
        ReadInput.WaitForUser();
    }
}

public class CreateFolderAction : INavigationAction
{
    public string Name => "stvori mapu";

    public void Execute(User user, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IUserService userService, IFileService fileService, IFolderService folderService,
        ICommentService commentService, ISharedItemService sharedItemService)
    {
        Console.Clear();
        var folderName = Helper.GetName("unesite naziv mape koju zelite stvoriti");
        if (folderName == null) return;

        CommandAction.CreateItem<Folder>(folderName, user, folderService, fileService);
        ReadInput.WaitForUser();
    }
}

public class DeleteFolderAction : INavigationAction
{
    public string Name => "izbrisi mapu";

    public void Execute(User user, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IUserService userService, IFileService fileService, IFolderService folderService,
         ICommentService commentService, ISharedItemService sharedItemService)
    {
        Console.Clear();
        var folderName = Helper.GetName("unesite naziv mape koju zelite izbrisati"); 
        if (folderName == null) return;

        string parts = $"izbrisi mapu '{folderName}'";

        CommandAction.GetDeleteFolderFile(parts.Split(" "), user, folderService, fileService, userService, userFolders, commentService, sharedItemService);

    }
}

public class DeleteFileAction : INavigationAction
{
    public string Name => "izbrisi datoteku";

    public void Execute(User user, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IUserService userService, IFileService fileService, IFolderService folderService,
         ICommentService commentService, ISharedItemService sharedItemService)
    {
        Console.Clear();
        var fileName = Helper.GetName("unesite naziv datoteke koju zelite izbrisati");
        if (fileName == null) return;

        string parts = $"izbrisi datoteku '{fileName}'";

        CommandAction.GetDeleteFolderFile(parts.Split(" "), user, folderService, fileService, userService, userFolders, commentService, sharedItemService);

    }
}

public class ChangeWorkingDirectory : INavigationAction
{
    public string Name => "udi u mapu";

    public void Execute(User user, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IUserService userService, IFileService fileService, IFolderService folderService,
         ICommentService commentService, ISharedItemService sharedItemService)
    {
        Console.Clear();
        var folderName = Helper.GetName("Unesite naziv mape u koju zelite uci");
        if (folderName == null) return;

        string parts = $"udi u mapu '{folderName}'";
        userFolders = userService.GetFoldersOrFiles<Folder>(user);

        CommandAction.GetChangeDirectory(parts.Split(" "), userFolders, userService, user, commentService);
    }
}

public class EnterFile : INavigationAction
{
    public string Name => "udi u datoteku";

    public void Execute(User user, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IUserService userService, IFileService fileService, IFolderService folderService,
         ICommentService commentService, ISharedItemService sharedItemService)
    {
        Console.Clear();
        var fileName = Helper.GetName("Unesite naziv datoteke u koju zelite uci");
        if (fileName == null) return;

        string parts = $"udi u datoteku '{fileName}'";

        userFiles = userService.GetFoldersOrFiles<File>(user);

        CommandAction.GetChangeDirectory(parts.Split(" "), userFolders, userService, user, commentService);
    }
}

public class EditFile : INavigationAction
{
    public string Name => "uredi datoteku";

    public void Execute(User user, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IUserService userService, IFileService fileService, IFolderService folderService,
         ICommentService commentService, ISharedItemService sharedItemService)
    {
        Console.Clear();
        var fileName = Helper.GetName("Unesite naziv datoteke koju zelite preimenovati");
        if (fileName == null) return;

        string parts = $"uredi datoteku '{fileName}'";

        CommandAction.GetEditFile(parts.Split(" "), user, fileService, userService);
    }
}

public class RenameFolder : INavigationAction
{
    public string Name => "promijeni naziv mape";

    public void Execute(User user, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IUserService userService, IFileService fileService, IFolderService folderService,
         ICommentService commentService, ISharedItemService sharedItemService)
    {
        Console.Clear();
        var folderName = Helper.GetName("unesite naziv mape koje zelite preimenovat");
        if (folderName == null) return;

        var newFolderName = Helper.GetName("unesite naziv nove mape");

        string parts = $"promijeni naziv mape '{folderName}' u '{newFolderName}'";

        CommandAction.GetRename(parts.Split(" "), userFolders, folderService, fileService, userService, user);
    }
}


public class RenameFile : INavigationAction
{
    public string Name => "promijeni naziv datoteke";

    public void Execute(User user, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IUserService userService, IFileService fileService, IFolderService folderService,
         ICommentService commentService, ISharedItemService sharedItemService)
    {
        Console.Clear();
        var fileName = Helper.GetName("unesite naziv datoteke koju zelite preimenovat");
        if (fileName == null) return;

        var newFileName = Helper.GetName("unesite novi nazic datoteke");
        if(newFileName == null) return;

        string parts = $"promijeni naziv datoteke '{fileName}' u '{newFileName}'";

        CommandAction.GetRename(parts.Split(" "), userFolders, folderService, fileService, userService, user);
    }
}

public class CurrentDir : INavigationAction
{
    public string Name => "trenutni direktorij";

    public void Execute(User user, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IUserService userService, IFileService fileService, IFolderService folderService,
         ICommentService commentService, ISharedItemService sharedItemService)
    {
        Console.WriteLine($"trenunto se nalazite u direktoriju: {CommandAction.CurrentDirectory()}");
        ReadInput.WaitForUser();
    }
}

public class UserFoldersAndFiles() : INavigationAction
{
    public string Name => "Vase mape i datoteke";

    public void Execute(User user, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IUserService userService, IFileService fileService, IFolderService folderService,
         ICommentService commentService, ISharedItemService sharedItemService)
    {
        userFolders = userService.GetFoldersOrFiles<Folder>(user);
        userFiles = userService.GetFoldersOrFiles<File>(user);
        Helper.ShowUserFoldersAndFiles(user, userService, userFolders, userFiles);
        ReadInput.WaitForUser();
    }
}

public class StartSharingFolderAction : INavigationAction
{
    public string Name => "podijeli mapu s";

    public void Execute(User user, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IUserService userService, IFileService fileService, IFolderService folderService,
         ICommentService commentService, ISharedItemService sharedItemService)
    {
        Console.Clear();
        var email = Helper.GetName("unesite email korisnika s kojim zelite podijeliti mapu");
        if (email == null) return;

        string parts = $"podijeli mapu s '{email}'";

        CommandAction.GetStartSharing(parts.Split(" "), userFolders, folderService, fileService, userService, sharedItemService, user, commentService);
    }
}

public class StartSharingFileAction : INavigationAction
{
    public string Name => "podijeli datoteku s";

    public void Execute(User user, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IUserService userService, IFileService fileService, IFolderService folderService,
         ICommentService commentService, ISharedItemService sharedItemService)
    {
        Console.Clear();
        var email = Helper.GetName("unesite email korisnika s kojim zelite podijeliti datoteku");
        if (email == null) return;

        string parts = $"podijeli datoteku s '{email}'";

        CommandAction.GetStartSharing(parts.Split(" "), userFolders, folderService, fileService, userService, sharedItemService, user, commentService);
    }
}
public class StopSharingFolderAction : INavigationAction
{
    public string Name => "prestani dijeliti mapu s";

    public void Execute(User user, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IUserService userService, IFileService fileService, IFolderService folderService,
         ICommentService commentService, ISharedItemService sharedItemService)
    {
        Console.Clear();
        var email = Helper.GetName("unesite email korisnika s kojim zelite prestati dijeliti mapu");
        if (email == null) return;

        string parts = $"prestani dijeliti mapu s '{email}'";

        CommandAction.GetStopSharing(parts.Split(" "), userFolders, folderService, fileService, userService, sharedItemService, user, commentService);
    }
}

public class StopSharingFileAction : INavigationAction
{
    public string Name => "prestani dijeliti datoteku s";

    public void Execute(User user, IEnumerable<Folder> userFolders, IEnumerable<File> userFiles, IUserService userService, IFileService fileService, IFolderService folderService,
         ICommentService commentService, ISharedItemService sharedItemService)
    {
        Console.Clear();
        var email = Helper.GetName("unesite email korisnika s kojim zelite prestati dijeliti datoteku");
        if (email == null) return;

        string parts = $"prestani dijeliti datoteku s '{email}'";

        CommandAction.GetStopSharing(parts.Split(" "), userFolders, folderService, fileService, userService, sharedItemService, user, commentService);
    }
}