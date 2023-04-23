using System.Security.Cryptography;
using System.Text;
using anuR.Services;

public class HashService : IHashService
{
    public string GetHash(string input)
    {
        // Convert the input string to a byte array and compute the hash.
        var data = SHA512.HashData(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        var sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data
        // and format each one as a hexadecimal string.
        foreach (var t in data)
            sBuilder.Append(t.ToString("x2"));

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    // Verify a hash against a string.
    public bool VerifyHash(string input, string hash)
    {
        HashAlgorithm hashAlgorithm = SHA512.Create();

        // Hash the input.
        var hashOfInput = GetHash(input);

        // Create a StringComparer an compare the hashes.
        var comparer = StringComparer.OrdinalIgnoreCase;

        return comparer.Compare(hashOfInput, hash) == 0;
    }
}