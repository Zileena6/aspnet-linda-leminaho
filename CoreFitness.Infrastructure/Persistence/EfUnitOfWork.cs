using CoreFitness.Domain.Exceptions;
using CoreFitness.Domain.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Persistence;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly CoreFitnessDbContext _context;

    public EfUnitOfWork(CoreFitnessDbContext context)
    {
        _context = context;
    }

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken ct = default)
    {
        var transaction = await _context.Database.BeginTransactionAsync(ct);
        return new EfTransaction(transaction);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        try
        {
            return await _context.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException(ex);
        }
        catch (DbUpdateException ex)
        {
            throw new PersistenceException(ex);
        }
    }
}