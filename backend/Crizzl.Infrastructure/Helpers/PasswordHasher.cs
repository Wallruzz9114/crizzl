using System.Security.Cryptography;
using System.Text;

namespace Crizzl.Infrastructure.Helpers
{
    public static class PasswordHasher
    {
        public static void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}