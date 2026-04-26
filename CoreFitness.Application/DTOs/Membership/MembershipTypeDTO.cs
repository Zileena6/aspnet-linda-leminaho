namespace CoreFitness.Application.DTOs.Membership;

public record MembershipTypeDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int DurationInDays { get; init; }
    public int SessionLimit { get; init; }
    public string Type { get; init; } = string.Empty;
    public IReadOnlyList<string> Benefits { get; init; } = [];
}
