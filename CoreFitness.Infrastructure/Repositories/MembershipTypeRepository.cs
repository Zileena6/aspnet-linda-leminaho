using CoreFitness.Domain.Entities.Memberships;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Interfaces.Memberships;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Repositories;

public class MembershipTypeRepository(CoreFitnessDbContext context) : BaseRepository<MembershipType, MembershipTypeId>(context),
        IMembershipTypeRepository
{
    public override async Task<IEnumerable<MembershipType>> GetAllAsync(CancellationToken ct = default) =>
        await _context.MembershipTypes
        .Include(mt => mt.Benefits)
        .AsNoTracking()
        .ToListAsync(ct);
}