using CoreFitness.Application.DTOs.User;
using CoreFitness.Application.Interfaces;
using CoreFitness.Domain.Common;

namespace CoreFitness.Application.Services;

public class AdminService(IAdminQueries adminQueries) : IAdminService
{
    public async Task<Result<IEnumerable<AdminUserDTO>>> GetAllUsersAsync(CancellationToken ct = default)
    {
        var users = await adminQueries.GetAllUsersAsync(ct);

        return Result<IEnumerable<AdminUserDTO>>.Success(users);
    }
}