using CoreFitness.Application.DTOs.Membership;
using CoreFitness.Domain.Common;
using CoreFitness.Domain.Entities.Users.ValueObjects;

namespace CoreFitness.Application.Interfaces;

public interface IMembershipService
{
    Task<Result<MembershipDTO>> GetByUserIdAsync(AuthenticationId authenticationId, CancellationToken ct = default);
    Task<Result<IEnumerable<MembershipTypeDTO>>> GetMembershipTypesAsync(CancellationToken ct = default);
    Task<Result> CreateAsync(AuthenticationId authenticationId, CreateMembershipDTO dto, CancellationToken ct = default);
    Task<Result> DeactivateAsync(AuthenticationId authenticationId, CancellationToken ct = default);
    Task<Result> ActivateAsync(AuthenticationId authenticationId, CancellationToken ct = default);
    Task<Result> CheckInAsync(Guid userId, CancellationToken ct = default);
    Task<Result> DeleteAsync(AuthenticationId authenticationId, CancellationToken ct = default);
}
