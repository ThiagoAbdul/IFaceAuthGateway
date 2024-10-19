using System.Security.Cryptography;

namespace IFaceAuthService.Helpers;

public class CryptoHelper
{

    private static readonly int _iterations = 1000;
    private static readonly int _saltSize = 16;
    private static readonly int _keySize = 32;
    private static readonly char _delimiter = ';';

    public static string GenerateHash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);
        byte[] hash = Hash(password, salt);
        return string.Join(_delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    private static byte[] Hash(string password, byte[] salt)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
            password: password,
            salt: salt,
            iterations: _iterations,
            hashAlgorithm: HashAlgorithmName.SHA256,
            outputLength: _keySize
        );
    }

    public static bool VerifyHash(string hashPassword, string inputPassword)
    {
        string[] elements = hashPassword.Split(_delimiter);
        byte[] salt = Convert.FromBase64String(elements[0]);
        byte[] hash = Convert.FromBase64String(elements[1]);
        byte[] hashInput = Hash(inputPassword, salt);
        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }
}