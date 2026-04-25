namespace CoreFitness.Domain.Interfaces;

public interface IBaseRepository<T, TId>
{
    Task<T?> GetByIdAsync(TId id, CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(TId id, CancellationToken ct = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
    Task<bool> ExistsByIdAsync(TId id, CancellationToken ct = default);
}
