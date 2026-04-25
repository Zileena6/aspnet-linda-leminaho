using CoreFitness.Domain.Entities.Memberships;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Entities.Users.ValueObjects;

namespace CoreFitness.Domain.Interfaces.Memberships;

public interface IMembershipRepository : IBaseRepository<Membership, MembershipId>
{
    Task<Membership?> GetByUserIdAsync(UserId userId, CancellationToken ct = default);
}
