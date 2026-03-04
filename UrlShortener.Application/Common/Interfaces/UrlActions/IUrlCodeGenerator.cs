namespace UrlShortener.Application.Common.Interfaces.UrlActions;

public interface IUrlCodeGenerator
{
    string GenerateCode(long id);
}