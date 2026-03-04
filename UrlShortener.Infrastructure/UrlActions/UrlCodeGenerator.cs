using System.Text;
using UrlShortener.Application.Common.Interfaces.UrlActions;

namespace UrlShortener.Infrastructure.UrlActions;

public class UrlCodeGenerator : IUrlCodeGenerator
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const long Salt = 0x5bd1e995;
    
    public string GenerateCode(long id)
    {
        var obfuscated = id ^ Salt;
        return EncodeBase62(obfuscated);
    }
    
    private static string EncodeBase62(long number)
    {
        if (number == 0)
            return Alphabet[0].ToString();

        var result = new StringBuilder();

        while (number > 0)
        {
            result.Insert(0, Alphabet[(int)(number % 62)]);
            number /= 62;
        }

        return result.ToString();
    }
}