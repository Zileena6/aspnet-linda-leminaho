using CoreFitness.Application.DTOs.User;

namespace CoreFitness.Application.Interfaces
{
    public interface IAdminQueries
    {
        Task<IEnumerable<AdminUserDTO>> GetAllUsersAsync(CancellationToken ct = default);
    }
}
