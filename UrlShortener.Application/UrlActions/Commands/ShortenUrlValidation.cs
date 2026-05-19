using FluentValidation;

namespace UrlShortener.Application.UrlActions.Commands;

public class ShortenUrlValidation : AbstractValidator<ShortenUrlCommand>
{
    public ShortenUrlValidation()
    {
        RuleFor(x => x.Url)
            .NotEmpty()
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("The Url shorten url is invalid.");
    }
}