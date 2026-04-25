using CoreFitness.Domain.Entities.Memberships;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Entities.Users.ValueObjects;
using CoreFitness.Domain.Interfaces.Memberships;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Repositories;

public class MembershipRepository(CoreFitnessDbContext context) : BaseRepository<Membership, MembershipId>(context),
        IMembershipRepository
{
    public async Task<Membership?> GetByUserIdAsync(UserId userId, CancellationToken ct = default) =>
        await _context.Memberships
            .Include(m => m.CheckIns)
            .FirstOrDefaultAsync(m => m.UserId.Equals(userId), ct);
}