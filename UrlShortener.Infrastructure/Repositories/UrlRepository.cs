using UrlShortener.Application.Common.Interfaces.Repositories;
using UrlShortener.Domain.UrlAggregate;

namespace UrlShortener.Infrastructure.Repositories;

public class UrlRepository : IUrlRepository
{
    private List<Url> _urls = new ();
    private static long _idCounter = 0;
    
    public void Add(Url url)
    {
        typeof(Url)
            .GetProperty("Id")!
            .SetValue(url, ++_idCounter);

        _urls.Add(url);
    }

    public void Update(Url url)
    {

    }

    public Task<Url?> GetByLongUrlAsync(string longUrl)
    {
        var url = _urls.FirstOrDefault(u => u.LongUrl == longUrl);
        return Task.FromResult(url);
    }

    public Task<Url?> GetCodeAsync(string code)
    {
        var url = _urls.FirstOrDefault((u => u.Code == code));
        return  Task.FromResult(url);
    }
}