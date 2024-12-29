using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Domain.Services;
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
        public static bool ConfirmPassword(string password)
        {
            while (true)
            {
                Console.WriteLine("Potvrdite lozinku:");
                var confirmedPassword = Console.ReadLine();

                if (string.IsNullOrEmpty(confirmedPassword))
                {
                    Console.WriteLine("Odustali ste od potvrde lozinke. Povratak...");
                    return false;
                }

                if (confirmedPassword != password)
                {
                    Console.WriteLine("Lozinke se ne podudaraju, pokušajte ponovno; za odustajanje ostavite prazno");
                    continue;
                }
                
                return true;
            }
        }
        public static void ShowUserFoldersAndFiles(User user, IUserService _userService, IEnumerable<Folder> userFolders, IEnumerable<Drive.Data.Entities.Models.File> userFiles)
        {
            

            if (!userFolders.Any() && !userFiles.Any())
                Console.WriteLine("Nemate kreiranih mapa i datoteka");


            Console.WriteLine("Vase datoteke: ");

            foreach (var folder in userFolders.OrderBy(folder => folder.Name))
            {
                FolderProcessesHelper.DisplayFolder(folder);
                FileProcessesHelper.DisplayFilesForFolder(userFiles, folder.Id);
                Console.WriteLine();
            }
        }
        public static (IEnumerable<Folder>, IEnumerable<Data.Entities.Models.File>) ShowSharedDataWithUser(ISharedItemService _sharedItemService, User _loggedUser)
        {
            var sharedFolders = _sharedItemService.GetAllSharedWithUser(_loggedUser, Data.Enums.DataType.Folder).OrderBy(f => f.Folder.Name);
            var sharedFiles = _sharedItemService.GetAllSharedWithUser(_loggedUser, Data.Enums.DataType.File).OrderBy(f => f.File.LastModifiedAt);
            var folders = new List<Folder>();
            var files = new List<Drive.Data.Entities.Models.File>();

            Console.WriteLine(" Podijeljene mape s vama: ");
            foreach (var folder in sharedFolders)
            {
                if (folder != null)
                {
                    Console.WriteLine($"\t-Mapa: {folder.Folder.Name} id mape: {folder.Folder.Id} podijeljena od korisnika: {folder.Folder.Owner.Name}");
                    folders.Add(folder.Folder);
                }
                else
                {
                    Console.WriteLine("\tNema mapa podijeljenih s vama");
                    break;
                }
            }

            Console.WriteLine("\n Podijeljene datoteke s vama: ");

            foreach (var file in sharedFiles)
            {
                if (file != null)
                {
                    Console.WriteLine($"\t-Datoteka: {file.File.Name} podijeljena od korisnika: {file.File.Owner.Name} unutar mape: {file.File.Folder.Name}, zadnji put promijenjena: {file.File.LastModifiedAt}");
                    files.Add(file.File);
                }
                else
                {
                    Console.WriteLine("\tNema datoteka podijeljenih s vama");
                    break;
                }
            }

            return(folders, files);
        }
        public static bool IsValidCommandStopSharing(string[] parts)
        {
            return parts.Length == 5 && parts[1] == "dijeliti" && (parts[2] == "mapu" || parts[2] == "datoteku") && parts[3] == "s";
        }
    }
}
