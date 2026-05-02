namespace CoreFitness.Application.Authentication.Models;

public class AuthenticationResult
{
    public AuthenticationResultType Type { get; init; }
    public string? ReturnUrl { get; init; }
    public string? Email { get; init; }

    public static AuthenticationResult Failed(string? returnUrl) => new()
    {
        Type = AuthenticationResultType.Failed,
        ReturnUrl = returnUrl
    };
    public static AuthenticationResult SignedIn(string? returnUrl) => new()
    {
        Type = AuthenticationResultType.SignedIn,
        ReturnUrl = returnUrl
    };
    public static AuthenticationResult RequiresVerification(string email,
    string? returnUrl) => new()
    {
        Type = AuthenticationResultType.RequiresVerification,
        ReturnUrl = returnUrl,
        Email = email
    };
    public static AuthenticationResult InvalidCode(string? returnUrl) => new()
    {
        Type = AuthenticationResultType.InvalidCode,
        ReturnUrl = returnUrl
    };
    public static AuthenticationResult RequiresAccountCreation(string email, string? returnUrl) => new()
    {
        Type = AuthenticationResultType.RequiresAccountCreation,
        Email = email,
        ReturnUrl = returnUrl
    };
}