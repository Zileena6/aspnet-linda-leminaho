using CoreFitness.Domain.Entities.Common;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Entities.Users.ValueObjects;

namespace CoreFitness.Domain.Entities.Memberships;

public class CheckIn : BaseEntity<CheckInId>
{
    public UserId UserId { get; private set; }
    public MembershipId MembershipId { get; private set; }
    public DateTimeOffset CheckedInAt { get; private set; }

    private CheckIn() { }

    private CheckIn(CheckInId id, UserId userId, MembershipId membershipId)
    {
        Id = id;
        UserId = userId;
        MembershipId = membershipId;
        CheckedInAt = DateTimeOffset.UtcNow;
    }

    public static CheckIn Create(UserId userId, MembershipId membershipId) =>
        new(CheckInId.New(), userId, membershipId);
}
