using ErrorOr;
using MediatR;
using UrlShortener.Application.UrlActions.Common;

namespace UrlShortener.Application.UrlActions.Queries.UrlQueries;

public record UrlQuery(
    string Url) : IRequest<ErrorOr<UrlResult>>; 