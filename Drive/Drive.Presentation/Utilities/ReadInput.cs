﻿using Drive.Domain.Interfaces;
using Drive.Presentation.Utilities;

namespace Drive.Presentation.Reader
{
    public static class ReadInput
    {
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
            do
            {
                Console.WriteLine(propmpt);
                var userInput = Console.ReadLine();

                if (string.IsNullOrEmpty(userInput))
                    return null;

                if (validate == null || validate(userInput))
                    return userInput;

                Console.WriteLine(message);
            } while (true);
        }

        public static bool IsValidEmail(string email)
        {
            if(string.IsNullOrEmpty(email)) return false;

            var parts = email.Split('@');
            if(parts.Length != 2) return false;

            string part1 = parts[0];
            string part2 = parts[1];

            if(part1.Length < 1) return false;
            
            var dotParts = part2.Split('.');
            if(dotParts.Length != 2) return false;

            string beforeDot = dotParts[0];
            string afterDot = dotParts[1];

            if(beforeDot.Length < 2) return false;
            if(afterDot.Length < 3) return false;

            return true;
        }

        public static string ?RegisterPassword(string prompt, Func<string, bool> ?validate = null, string message = "Ne ispravan unos")
        {
            Console.WriteLine(prompt);
            var password = Console.ReadLine();


            if(string.IsNullOrEmpty(password) && validate != null && !validate(password))
                return null;

            while (true)
            {
                Console.WriteLine("Potvrdite lozinku:");
                var confirmedPassword = Console.ReadLine();

                if(string.IsNullOrEmpty(confirmedPassword))
                {
                    Console.WriteLine("Odustali ste od potvrde lozinke. Povratak...");
                    return null;
                }

                if (confirmedPassword == password)
                {
                    return password;
                }
                Console.WriteLine("Lozinke se ne podudaraju, pokušajte ponovno; za odustajanje ostavite prazno");
            }

        }

        public static bool CheckUserPassword(string email, IUserService userService)
        {
            Console.WriteLine("Unesite vasu lozinku: ");
            var password = Console.ReadLine();

            if(string.IsNullOrEmpty(password) )
                return false;

            var hashedPassword = Hash.HashText(password);
           
            return userService.PasswordsMatch(email, hashedPassword);            
        }

        public static bool ConfirmAction(string prompt)
        {
            Console.WriteLine(prompt + "y/n");

            return Console.ReadLine() == "y";
        }
    }
}
