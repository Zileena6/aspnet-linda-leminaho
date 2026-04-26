namespace CoreFitness.Application.DTOs.Membership;

public record CheckInDTO
{
    public Guid Id { get; init; }
    public DateTimeOffset CheckedInAt { get; init; }
}
