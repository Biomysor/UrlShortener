namespace UrlShortener.Contracts.Authentication;

public record RegisterRequest(
    string Login,
    string Email, 
    string Password);