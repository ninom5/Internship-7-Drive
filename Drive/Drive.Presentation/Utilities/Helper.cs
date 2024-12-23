using Drive.Data.Entities.Models;

namespace Drive.Presentation.Utilities
{
    public class Helper
    {
        public static bool IsAncestor(Folder currentFolder, string folderToDeleteName, IEnumerable<Folder> allFolders)
        {
            while (currentFolder.ParentFolderId != null)
            {
                var parentFolder = allFolders.FirstOrDefault(f => f.Id == currentFolder.ParentFolderId);

                if (parentFolder == null)
                    return false;

                if (parentFolder.Name == folderToDeleteName)
                    return true;

                currentFolder = parentFolder;
            }

            return false;
        }
    }
}
