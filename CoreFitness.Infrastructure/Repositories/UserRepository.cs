using CoreFitness.Domain.Entities.Users;
using CoreFitness.Domain.Entities.Users.ValueObjects;
using CoreFitness.Domain.Interfaces.Users;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Repositories;

public class UserRepository(CoreFitnessDbContext context) : BaseRepository<User, UserId>(context),
        IUserRepository
{
    public async Task<bool> ExistsByEmailAsync(UserEmail email, CancellationToken ct = default) =>
        await _context.Users.AnyAsync(e => e.EmailUnique == email.UniqueValue, ct);

    public async Task<User?> GetByAuthenticationIdAsync(AuthenticationId id, CancellationToken ct = default) =>
        await _context.Users.FirstOrDefaultAsync(u => u.AuthenticationId == id, ct);

    public async Task<User?> GetByEmailAsync(UserEmail email, CancellationToken ct = default) =>
        await _context.Users.FirstOrDefaultAsync(u => u.EmailUnique == email.UniqueValue);
}