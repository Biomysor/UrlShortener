using MediatR;
using ErrorOr;

namespace UrlShortener.Application.UrlActions.Queries.RedirectQueries;

public record RedirectQuery(
    string Code) : IRequest<ErrorOr<string>>;