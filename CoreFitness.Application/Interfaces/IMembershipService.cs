using CoreFitness.Application.DTOs.Membership;
using CoreFitness.Domain.Common;

namespace CoreFitness.Application.Interfaces;

public interface IMembershipService
{
    Task<Result<MembershipDTO>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<Result<IEnumerable<MembershipTypeDTO>>> GetMembershipTypesAsync(CancellationToken ct = default);
    Task<Result> CreateAsync(Guid userId, CreateMembershipDTO dto, CancellationToken ct = default);
    Task<Result> DeactivateAsync(Guid userId, CancellationToken ct = default);
    Task<Result> ActivateAsync(Guid userId, CancellationToken ct = default);
    Task<Result> CheckInAsync(Guid userId, CancellationToken ct = default);
}
