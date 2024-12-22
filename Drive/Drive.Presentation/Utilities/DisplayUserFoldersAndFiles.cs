
using Drive.Data.Entities.Models;

namespace Drive.Presentation.Utilities
{
    public class DisplayUserFoldersAndFiles
    {
        public static void DisplayFolder(Folder folder)
        {
            if (folder.ParentFolder != null)
            {
                Console.WriteLine($"Mapa: {folder.Name}, Id mape: {folder.Id}, Parent folder id: {folder.ParentFolderId}, naziv: {folder.ParentFolder.Name}");
            }
            else
            {
                Console.WriteLine($"Mapa: {folder.Name}, Id mape: {folder.Id}");
            }
        }

        public static void DisplayFilesForFolder(IEnumerable<Drive.Data.Entities.Models.File> files, int folderId)
        {
            var folderFiles = files
                .Where(file => file.FolderId == folderId)
                .OrderBy(file => file.LastModifiedAt);

            foreach (var file in folderFiles)
            {
                Console.WriteLine($"\tFile: {file.Name}, id mape: {file.FolderId}, zadnji put promijenjeno: {file.LastModifiedAt}");
            }
        }
    }
}
