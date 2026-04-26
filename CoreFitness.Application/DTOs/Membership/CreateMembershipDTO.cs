namespace CoreFitness.Application.DTOs.Membership;

public record CreateMembershipDTO
{
    public Guid MembershipTypeId { get; init; }
}
