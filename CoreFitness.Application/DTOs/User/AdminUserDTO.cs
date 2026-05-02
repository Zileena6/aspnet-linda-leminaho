using CoreFitness.Application.DTOs.Membership;

namespace CoreFitness.Application.DTOs.User;

public record AdminUserDTO
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public MembershipDTO? Membership { get; init; }
}