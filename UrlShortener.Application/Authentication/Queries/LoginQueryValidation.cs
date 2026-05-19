using FluentValidation;

namespace UrlShortener.Application.Authentication.Queries;

public class LoginQueryValidation : AbstractValidator<LoginQuery>
{
    public LoginQueryValidation()
    {
        RuleFor(registerCommand => registerCommand.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress();
        
        RuleFor(registerCommand => registerCommand.Password)
            .MaximumLength(24).WithMessage("Password must be 24 characters or less.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"^(?=.*[A-Z])(?=.*\d).+$")
            .WithMessage("Password must contain at least one uppercase letter and one digit.")
            .NotEmpty().WithMessage("Password is required.");
    }
}