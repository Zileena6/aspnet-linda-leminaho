namespace CoreFitness.Application.Authentication.Models;

public record ExternalUserInfo
{
    public string Provider { get; init; } = default!;
    public string ProviderKey { get; init; } = default!;

    public string? Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? PhotoUrl { get; init; }
}