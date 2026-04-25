using CoreFitness.Domain.Entities.Users;
using CoreFitness.Domain.Entities.Users.ValueObjects;

namespace CoreFitness.Domain.Interfaces.Users;

public interface IUserRepository : IBaseRepository<User, UserId>
{
    Task<User?> GetByEmailAsync(UserEmail email, CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(UserEmail email, CancellationToken ct = default);
}
