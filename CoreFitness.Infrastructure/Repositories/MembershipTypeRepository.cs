using CoreFitness.Domain.Entities.Memberships;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Enums;
using CoreFitness.Domain.Interfaces.Memberships;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Repositories;

public class MembershipTypeRepository(CoreFitnessDbContext context) : BaseRepository<MembershipType, MembershipTypeId>(context),
        IMembershipTypeRepository
{
    public async Task<MembershipType?> GetByTypeAsync(MembershipTypeEnums type, CancellationToken ct = default)
    {
        return await _context.MembershipTypes.FirstOrDefaultAsync(mt => mt.Type == type, ct);
    }
}