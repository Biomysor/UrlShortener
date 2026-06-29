using System.Text;
using UrlShortener.Application.Common.Interfaces.UrlActions;
using UrlShortener.Domain.UrlAggregate.ValueObjects;

namespace UrlShortener.Infrastructure.UrlActions;

public class UrlCodeGenerator : IUrlCodeGenerator
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    
    public string GenerateCode(UrlId id)
    {
        var bytes = id.Value.ToByteArray();
        var number = Math.Abs(BitConverter.ToInt64(bytes, 0));

        return EncodeBase62(number);
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