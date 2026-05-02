using CoreFitness.Application.DTOs.Membership;
using CoreFitness.Application.Interfaces;
using CoreFitness.Application.Mappings;
using CoreFitness.Domain.Common;
using CoreFitness.Domain.Entities.Memberships;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Interfaces.Memberships;
using CoreFitness.Domain.Interfaces.UnitOfWork;

namespace CoreFitness.Application.Services;

public class MembershipTypeService(
    IMembershipTypeRepository repository, 
    IMembershipRepository membershipRepository, 
    IUnitOfWork unitOfWork) : IMembershipTypeService
{
    public async Task<Result> AddBenefitAsync(Guid membershipTypeId, string description, CancellationToken ct = default)
    {
        var type = await repository.GetByIdAsync(new MembershipTypeId(membershipTypeId), ct);

        if (type is null)
            return Result.NotFound("MembershipType", membershipTypeId);

        type.AddBenefit(MembershipTypeBenefitDescription.Create(description));

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result<MembershipTypeDTO>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var type = await repository.GetByIdAsync(new MembershipTypeId(id), ct);

        if (type is null)
            return Result<MembershipTypeDTO>.NotFound("MembershipType", id);

        return Result<MembershipTypeDTO>.Success(type.ToDTO());
    }

    public async Task<Result<MembershipTypeDTO>> CreateAsync(CreateMembershipTypeDTO dto, CancellationToken ct = default)
    {
        var type = MembershipType.Create(
            MembershipTypeName.Create(dto.Name),
            MembershipTypeDescription.Create(dto.Description),
            MembershipTypePrice.Create(dto.Price),
            MembershipTypeDuration.Create(dto.DurationInDays),
            dto.SessionLimit
        );

        await repository.AddAsync(type, ct);

        await unitOfWork.SaveChangesAsync(ct);

        return Result<MembershipTypeDTO>.Success(type.ToDTO());
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var hasActive = await membershipRepository.HasActiveMembershipsByTypeAsync(new MembershipTypeId(id), ct);

        if (hasActive)
            return Result.Conflict("Cannot delete type with active memberships");

        var deleted = await repository.DeleteAsync(new MembershipTypeId(id), ct);

        if (!deleted)
            return Result.Failure(Error.NotFound("MembershipType", id));

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<MembershipTypeDTO>>> GetAllAsync(CancellationToken ct = default)
    {
        var types = await repository.GetAllAsync(ct);

        return Result<IEnumerable<MembershipTypeDTO>>.Success(types.Select(t => t.ToDTO()));
    }

    public async Task<Result> RemoveBenefitAsync(Guid membershipTypeId, Guid benefitId, CancellationToken ct = default)
    {
        var type = await repository.GetByIdAsync(new MembershipTypeId(membershipTypeId), ct);

        if (type is null)
            return Result.NotFound("MembershipType", membershipTypeId);

        type.RemoveBenefit(new MembershipTypeBenefitId(benefitId));

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(UpdateMembershipTypeDTO dto, CancellationToken ct = default)
    {
        var type = await repository.GetByIdAsync(new MembershipTypeId(dto.Id), ct);

        if (type is null)
            return Result.NotFound("MembershipType", dto.Id);

        type.UpdateName(MembershipTypeName.Create(dto.Name));
        type.UpdateDescription(MembershipTypeDescription.Create(dto.Description));
        type.UpdatePrice(MembershipTypePrice.Create(dto.Price));
        type.UpdateDuration(MembershipTypeDuration.Create(dto.DurationInDays));
        type.UpdateSessionLimit(dto.SessionLimit);

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}