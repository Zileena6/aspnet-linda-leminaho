using CoreFitness.Application.DTOs.User;
using CoreFitness.Domain.Common;

namespace CoreFitness.Application.Interfaces;

public interface IUserService
{
    Task<Result<UserDTO>> GetByIdAsync(Guid userId, CancellationToken ct = default);
    Task<Result> UpdateAsync(UpdateUserDTO dto, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid userId, CancellationToken ct = default);
    Task<Result> CompleteRegistrationAsync(Guid authenticationId, CompleteProfileDTO dto, CancellationToken ct = default);
}
