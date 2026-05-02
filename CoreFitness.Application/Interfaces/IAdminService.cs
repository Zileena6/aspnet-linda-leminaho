using CoreFitness.Application.DTOs.User;
using CoreFitness.Domain.Common;

namespace CoreFitness.Application.Interfaces;

public interface IAdminService
{
    Task<Result<IEnumerable<AdminUserDTO>>> GetAllUsersAsync(CancellationToken ct = default);
}