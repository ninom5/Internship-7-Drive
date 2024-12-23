using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces;
using Drive.Presentation.Reader;

namespace Drive.Presentation.Utilities
{
    public class Helper
    {
        public static string CollectMail(IUserService _userService)
        {
            while (true)
            {
                var email = ReadInput.ReadString("Unesite email: ", input => ReadInput.IsValidEmail(input), "Email mora biti u formatu [string min 1 chara]@[string min 2 chara].[string min 3 chara]\n");

                if (email == null)
                {
                    Console.WriteLine("Email ne moze biti prazan. Povratak...");
                    return "";
                }

                if (!_userService.EmailExists(email))
                    return email;

                Console.WriteLine("Uneseni email vec postoji");
            }
        }
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
