using FluentValidation;

namespace UrlShortener.Application.UrlActions.Queries.UrlQueries;

public class UrlQueryValidation : AbstractValidator<UrlQuery>
{
    public UrlQueryValidation()
    {
        RuleFor(x => x.Url)
            .NotEmpty()
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("The Url is invalid.");
    }
}