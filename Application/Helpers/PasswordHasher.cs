using System.Security.Cryptography;
using System.Text;

namespace Application.Helpers;

public class PasswordHasher
{
    private const int SaltSize = 128 / 8;

    public static string HashPassword(string password, out string salt)
    {
        byte[] saltBytes = GenerateSalt();
        salt = Convert.ToBase64String(saltBytes);
        byte[] hashBytes = ComputeHash(password, saltBytes);

        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerifyPassword(string password, string hashedPassword, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        byte[] computedHash = ComputeHash(password, saltBytes);

        return Convert.ToBase64String(computedHash) == hashedPassword;
    }

    private static byte[] GenerateSalt()
    {
        byte[] saltBytes = new byte[SaltSize];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);

        return saltBytes;
    }

    private static byte[] ComputeHash(string password, byte[] saltBytes)
    {
        using var hmac = new HMACSHA256(saltBytes);
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        return hmac.ComputeHash(passwordBytes);
    }
}
