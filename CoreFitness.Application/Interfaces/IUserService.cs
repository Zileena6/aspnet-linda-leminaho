using CoreFitness.Application.DTOs.User;
using CoreFitness.Domain.Common;
using CoreFitness.Domain.Entities.Users.ValueObjects;

namespace CoreFitness.Application.Interfaces;

public interface IUserService
{
    Task<Result<UserDTO>> GetByIdAsync(Guid userId, CancellationToken ct = default);
    Task<Result> UpdateAsync(UpdateUserDTO dto, CancellationToken ct = default);
    Task<Result> UpdateProfileAsync(UpdateProfileDTO dto, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid userId, CancellationToken ct = default);
    Task<Result> CompleteRegistrationAsync(AuthenticationId authId, CompleteProfileDTO dto, CancellationToken ct = default);
    Task<Result<UserStatisticsDTO>> GetStatisticsAsync(Guid userId, CancellationToken ct = default);
    Task<Result<UserDTO>> GetByAuthenticationId(AuthenticationId authId, CancellationToken ct = default);
    Task<Result> UpdateWeightAsync(Guid userId, decimal weight, decimal height, CancellationToken ct = default);
    Task<Result> UpdateWeightGoalAsync(Guid userId, decimal targetWeight, CancellationToken ct = default);
    Task<Result> UploadProfilePhotoAsync(AuthenticationId authId, Stream stream, string originalFileName, CancellationToken ct = default);
    Task<Result> DeleteAccountAsync(AuthenticationId authId, CancellationToken ct = default);
}
