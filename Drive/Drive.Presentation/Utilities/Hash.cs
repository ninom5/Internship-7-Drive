
using System.Security.Cryptography;
using System.Text;


namespace Drive.Presentation.Utilities
{
    public static class Hash
    {
        public static byte[] HashText(string password)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                var passwordHash = Encoding.UTF8.GetBytes(password);

                return sha512.ComputeHash(passwordHash);
            }
        }
    }
}
