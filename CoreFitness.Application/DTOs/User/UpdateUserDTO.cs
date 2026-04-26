namespace CoreFitness.Application.DTOs.User;

public record UpdateUserDTO
{
    public Guid Id { get; init; }
    public string? Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? PhoneNumber { get; init; }
    public string? PhotoUrl { get; init; }
    public byte[] RowVersion { get; init; } = default!;
}
