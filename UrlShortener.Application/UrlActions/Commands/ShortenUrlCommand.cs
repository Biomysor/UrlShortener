using MediatR;
using ErrorOr;
using UrlShortener.Application.UrlActions.Common;

namespace UrlShortener.Application.UrlActions.Commands;

public record ShortenUrlCommand(
    string Url) : IRequest<ErrorOr<UrlResult>>;