using CoreFitness.Application.DTOs.Membership;
using CoreFitness.Application.Interfaces;
using CoreFitness.Application.Mappings;
using CoreFitness.Domain.Common;
using CoreFitness.Domain.Entities.Memberships;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Entities.Users.ValueObjects;
using CoreFitness.Domain.Interfaces.Memberships;
using CoreFitness.Domain.Interfaces.UnitOfWork;

namespace CoreFitness.Application.Services;

public class MembershipService(
    IMembershipRepository repository, 
    IMembershipTypeRepository membershipTypeRepository, 
    IUnitOfWork unitOfWork) : IMembershipService
{
    public async Task<Result> ActivateAsync(Guid userId, CancellationToken ct = default)
    {
        var membership = await repository.GetByUserIdAsync(new UserId(userId), ct);
        if (membership is null)
            return Result.NotFound("Membership", userId);

        membership.ActivateMembership();

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> CreateAsync(Guid userId, CreateMembershipDTO dto, CancellationToken ct = default)
    {
        var membershipType = await membershipTypeRepository.GetByIdAsync(new MembershipTypeId(dto.MembershipTypeId), ct);
        if (membershipType is null)
            return Result.NotFound("MembershipType", dto.MembershipTypeId);

        var existing = await repository.GetByUserIdAsync(new UserId(userId), ct);
        if (existing is not null && existing.IsActive)
            return Result.Conflict("User already has an active membership");

        var startDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var endDate = startDate.AddDays(membershipType.Duration.Value);

        var membership = Membership.Create(
            new UserId(userId),
            membershipType.Id,
            membershipType.Price.Value,
            startDate,
            endDate,
            membershipType.SessionLimit
            );

        await repository.AddAsync(membership, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeactivateAsync(Guid userId, CancellationToken ct = default)
    {
        var membership = await repository.GetByUserIdAsync(new UserId(userId), ct);
        if (membership is null)
            return Result.NotFound("Membership", userId);

        membership.DeactivateMembership();

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result<MembershipDTO>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var membership = await repository.GetByUserIdAsync(new UserId(userId), ct);
        if (membership is null)
            return Result<MembershipDTO>.NotFound("Membership", userId);

        var membershipType = await membershipTypeRepository.GetByIdAsync(membership.TypeId, ct);
        if (membershipType is null)
            return Result<MembershipDTO>.NotFound("MembershipType", membership.TypeId);

        return Result<MembershipDTO>.Success(membership.ToDTO(membershipType.Name.Value, membershipType.Price.Value));
    }

    public async Task<Result<IEnumerable<MembershipTypeDTO>>> GetMembershipTypesAsync(CancellationToken ct = default)
    {
        var types = await membershipTypeRepository.GetAllAsync(ct);

        return Result<IEnumerable<MembershipTypeDTO>>.Success(types.Select(t => t.ToDTO()));
    }

    public async Task<Result> CheckInAsync(Guid userId, CancellationToken ct = default)
    {
        var membership = await repository.GetByUserIdAsync(new UserId(userId), ct);

        if (membership is null)
            return Result.NotFound("Membership", userId);

        membership.RegisterCheckIn();

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}