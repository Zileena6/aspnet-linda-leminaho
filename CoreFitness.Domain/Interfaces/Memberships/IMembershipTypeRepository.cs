using CoreFitness.Domain.Entities.Memberships;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Enums;

namespace CoreFitness.Domain.Interfaces.Memberships;

public interface IMembershipTypeRepository : IBaseRepository<MembershipType, MembershipTypeId>
{
    Task<MembershipType?> GetByTypeAsync(MembershipTypeEnums type, CancellationToken ct = default!);
}
