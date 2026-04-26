namespace CoreFitness.Application.DTOs.User;

public record CompleteProfileDTO
{
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
    public string? PhotoUrl { get; init; }
}
