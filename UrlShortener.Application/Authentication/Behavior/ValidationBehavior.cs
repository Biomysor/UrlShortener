using FluentValidation;
using MediatR;
using ErrorOr;

namespace UrlShortener.Application.Authentication.Behavior;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var failures = validators
            .Select(v => v.Validate(context))
            .SelectMany(v => v.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count == 0) return await next(cancellationToken);
        {
            var errors = failures
                .ConvertAll(f =>
                    Error.Validation(
                        code: f.PropertyName,
                        description: f.ErrorMessage));
            
            return (dynamic)errors;
        }

    }
    
}