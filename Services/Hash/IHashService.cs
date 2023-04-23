namespace anuR.Services;

public interface IHashService
{
    public string GetHash(string input);
    public bool VerifyHash(string input, string hash);
}