using CoreFitness.Application.DTOs.Membership;
using CoreFitness.Domain.Common;

namespace CoreFitness.Application.Interfaces;

public interface IMembershipTypeService
{
    Task<Result<IEnumerable<MembershipTypeDTO>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<MembershipTypeDTO>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<MembershipTypeDTO>> CreateAsync(CreateMembershipTypeDTO dto, CancellationToken ct = default);
    Task<Result> UpdateAsync(UpdateMembershipTypeDTO dto, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<Result> AddBenefitAsync(Guid membershipTypeId, string description, CancellationToken ct = default);
    Task<Result> RemoveBenefitAsync(Guid membershipTypeId, Guid benefitId, CancellationToken ct = default);
}