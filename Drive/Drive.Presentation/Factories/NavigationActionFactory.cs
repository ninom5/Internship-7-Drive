using Drive.Presentation.Interfaces;

public class NavigationActionFactory
{
    private readonly List<INavigationAction> _actions;

    public NavigationActionFactory()
    {
        _actions = new List<INavigationAction>
        {
            new CreateFileAction(),
            new CreateFolderAction(),
            new DeleteFolderAction(),
            new DeleteFileAction(),
            new ChangeWorkingDirectory(),
            new EnterFile(),
            new EditFile(),
            new RenameFolder(),
            new RenameFile(),
            new CurrentDir(),
            new UserFoldersAndFiles(),
            new StartSharingFolderAction(),
            new StartSharingFileAction(),
            new StopSharingFolderAction(),
            new StopSharingFileAction(),
        };
    }

    public INavigationAction GetAction(string name)
    {
        return _actions.FirstOrDefault(a => a.Name == name);
    }

    public List<string> GetActionNames()
    {
        return _actions.Select(a => a.Name).ToList();
    }
}
