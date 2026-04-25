using CoreFitness.Domain.Entities.Common;
using CoreFitness.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Repositories;

public abstract class BaseRepository<T, TId>(CoreFitnessDbContext context)
        where T : BaseEntity<TId>, IAggregateRoot
{
    protected readonly CoreFitnessDbContext _context = context;

    public virtual async Task AddAsync(T entity, CancellationToken ct = default) =>
        await _context.Set<T>().AddAsync(entity, ct);

    public virtual async Task<bool> DeleteAsync(TId id, CancellationToken ct = default)
    {
        var entity = await _context.Set<T>().FindAsync([id], ct);

        if (entity is null) return false;

        _context.Set<T>().Remove(entity);
        return true;
    }

    public virtual async Task<T?> GetByIdAsync(TId id, CancellationToken ct = default) =>
        await _context.Set<T>().FindAsync([id], ct);

    public virtual async Task<bool> ExistsByIdAsync(TId id, CancellationToken ct = default) =>
        await _context.Set<T>().AnyAsync(e => e.Id!.Equals(id), ct);

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Set<T>()
            .AsNoTracking()
            .ToListAsync(ct);
    }
}