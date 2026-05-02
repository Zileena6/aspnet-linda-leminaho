namespace CoreFitness.Application.DTOs.User;

public record UpdateProfileDTO
{
    public Guid Id { get; init; }
    public string? Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? PhoneNumber { get; init; }

    public decimal? Weight { get; init; }
    public decimal? Height { get; init; }
    public decimal? TargetWeight { get; init; }

    public byte[] RowVersion { get; init; } = default!;
}