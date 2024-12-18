
namespace Drive.Presentation.Utilities
{
    public static class Captcha
    {
        public static string GenerateCaptcha()
        {
            Random random = new Random();

            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            string letters = "abcdefghijklmnopqrstuuvwxyz";

            char[] captcha = new char[5];

            captcha[0] = numbers[random.Next(numbers.Length)].ToString()[0];
            captcha[1] = letters[random.Next(letters.Length)];

            for (int i = 2; i < captcha.Length; i++)
            {
                if(random.Next(2) == 0)
                {
                    captcha[i] = numbers[random.Next(numbers.Length)].ToString()[0];
                }
                else
                {
                    captcha[i] = letters[random.Next(letters.Length)];
                }
            }

            return new string(captcha);
        }
        public static bool ValidateCaptcha(string captcha)
        {
            while (true)
            {
                var userInput = Console.ReadLine();
                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("odustali ste od unosa");
                    return false;
                }

                if (captcha != userInput)
                {
                    Console.WriteLine("Ne poklapa se. Unesite opet");
                    continue;
                }

                Console.WriteLine("Uspjesno ste potvrdili");
                return true;
            }
        }
    }
}
