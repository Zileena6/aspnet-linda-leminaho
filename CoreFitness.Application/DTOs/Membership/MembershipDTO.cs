namespace CoreFitness.Application.DTOs.Membership;

public record MembershipDTO
{
    public Guid Id { get; init; }
    public string MembershipTypeName { get; init; } = string.Empty;
    public decimal PurchasedPrice { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public bool IsActive { get; init; }
    public bool IsExpired { get; init; }
    public bool IsManuallyDeactivated { get; init; }
    public int SessionsUsed { get; init; }
    public int SessionLimit { get; init; }
    public int SessionsRemaining => SessionLimit - SessionsUsed;
    public int CheckInsLast30Days { get; init; }
}