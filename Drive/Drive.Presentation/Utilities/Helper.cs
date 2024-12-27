using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
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
    }
}
