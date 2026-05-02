namespace CoreFitness.Application.DTOs.Membership;

public record CreateMembershipTypeDTO
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int DurationInDays { get; init; }
    public int SessionLimit { get; init; }
}