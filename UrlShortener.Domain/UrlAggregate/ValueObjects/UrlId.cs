using UrlShortener.Domain.Common;

namespace UrlShortener.Domain.UrlAggregate.ValueObjects;

public sealed class UrlId : ValueObject
{
    public Guid Value { get; }
    
    private UrlId(Guid value)
    {
        Value = value;
    }

    public static UrlId CreateUnique()
    {
        return new UrlId(Guid.NewGuid());
    }
    
    public static UrlId Create( Guid value )
    {
        return new UrlId(value);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return  Value;
    }
}