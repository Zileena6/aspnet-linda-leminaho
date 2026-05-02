namespace CoreFitness.Application.Authentication.Models;

public sealed record ExternalSignInResult
(
    bool Succeeded,
    bool RequiresLinking = false,
    string? Error = null
)
{
    public static ExternalSignInResult Success() =>
        new(true, false, null);

    public static ExternalSignInResult NeedsLinking() =>
        new(false, true, null);

    public static ExternalSignInResult Failed(string? error = null) =>
        new(false, false, error);
}