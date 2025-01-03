using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Utilities;
using System.Text;
using System.Text.RegularExpressions;

namespace Drive.Presentation.Reader
{
    public static class ReadInput
    {
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
        public static string GetName(string prompt)
        {
            Console.WriteLine(prompt + ". Prazno za odustat");

            var name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("ne moze biti prazno. Povratak...");
                return "";
            }

            return name;
        }
        public static User FindUser(IUserService _userService, string email)
        {
            while (true)
            {
                if (!_userService.EmailExists(email))
                {
                    Console.WriteLine("Uneseni email ne postoji. Unesite novi email ili ostavite prazno za odustat");
                    email = Console.ReadLine()?.Trim();

                    if (string.IsNullOrEmpty(email))
                    {
                        Console.WriteLine("Odustali ste od unosa emaila.");
                        return null;
                    }

                    continue;
                }

                var userToShare = _userService.GetUser(email);

                if (userToShare == null)
                {
                    Console.WriteLine("Uneseni korisnik nije pronaden. Unesite novi email ili ostavite prazno za odustat");
                    email = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(email))
                    {
                        Console.WriteLine("Odustali ste od unosa emaila.");
                        return null;
                    }

                    continue;
                }

                return userToShare;
            }
        }
        public static string ReadFileContent()
        {
            Console.WriteLine("Unesite sadrzaj datoteke");
            StringBuilder stringBuilder = new StringBuilder();

            while (true)
            {
                var lineOfContent = Console.ReadLine();
                if (string.IsNullOrEmpty(lineOfContent))
                    break;

                stringBuilder.AppendLine(lineOfContent);
            }

            var content = stringBuilder.ToString();
            if (string.IsNullOrEmpty(content))
            {
                Console.WriteLine("Sadrzaj ne moze biti prazan. Povratak...");
                return "";
            }

            return content;
        }
        public static int ReadNumberChoice(string prompt, int min, int max)
        {
            do
            {
                Console.WriteLine(prompt);

                if (int.TryParse(Console.ReadLine(), out var result) && result >= min && result <= max)
                    return result;

                Console.WriteLine($"Neispravan unos, odaberite broj izmedu {min} i {max}");
            } while (true);
        }

        public static string ?ReadString(string propmpt, Func<string, bool> ?validate = null, string message = "Ne ispravan unos")
        {
            while(true)
            {
                Console.WriteLine(propmpt);
                var userInput = Console.ReadLine();

                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("Podatak ne moze biti prazan. Povratak na glavni menu...");    
                    return null; 
                }
                
                if (validate != null && !validate(userInput))
                {
                    Console.WriteLine(message);
                    continue;
                }

                return userInput?.Trim();
            }
        }

        public static string ?RegisterPassword(string prompt, Func<string, bool> ?validate = null, string message = "Ne ispravan unos")
        {
            while (true)
            {
                Console.WriteLine(prompt);
                var password = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(password) || (validate != null && !validate(password)))
                    return null;

                if (password.Length < 8)
                {
                    Console.WriteLine(message + ". Lozinka ne moze biti kraca od 8 znakova");
                    continue;
                }


                if (!ConfirmPassword(password))
                    return null;

                return password;
            }
        }

        public static bool CheckUserPassword(string email, IUserService userService)
        {
            Console.WriteLine("\nUnesite vasu lozinku: ");
            var password = Console.ReadLine()?.Trim();

            if(string.IsNullOrEmpty(password) )
                return false;

            var hashedPassword = Hash.HashText(password);
           
            return userService.PasswordsMatch(email, hashedPassword);            
        }

        public static void WaitForUser()
        {
            Console.WriteLine("Pritisnite tipku za nastavak");
            Console.ReadKey();

            Console.Clear();
        }
        public static bool ConfirmAction(string prompt)
        {
            Console.WriteLine(prompt + "y/n");

            return Console.ReadLine() == "y";
        }
    }
}
