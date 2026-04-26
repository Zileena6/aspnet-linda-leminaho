using CoreFitness.Domain.Enums;

namespace CoreFitness.Application.DTOs.User;

public record AdminCreateUserDTO
{
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
    public string? PhotoUrl { get; init; }
    public UserRole Role { get; init; }
}
