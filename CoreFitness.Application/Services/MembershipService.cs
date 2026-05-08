using CoreFitness.Application.DTOs.Membership;
using CoreFitness.Application.Interfaces;
using CoreFitness.Application.Mappings;
using CoreFitness.Domain.Common;
using CoreFitness.Domain.Entities.Memberships;
using CoreFitness.Domain.Entities.Memberships.ValueObjects;
using CoreFitness.Domain.Entities.Users.ValueObjects;
using CoreFitness.Domain.Interfaces.Memberships;
using CoreFitness.Domain.Interfaces.TrainingSessions;
using CoreFitness.Domain.Interfaces.UnitOfWork;
using CoreFitness.Domain.Interfaces.Users;

namespace CoreFitness.Application.Services
{
    public class MembershipService(IMembershipRepository repository, IMembershipTypeRepository membershipTypeRepository, ITrainingSessionRepository trainingSessionRepository, IUserRepository userRepository, IUnitOfWork unitOfWork) : IMembershipService
    {
        public async Task<Result> ActivateAsync(AuthenticationId authenticationId, CancellationToken ct = default)
        {
            var user = await userRepository.GetByAuthenticationIdAsync(authenticationId, ct);

            if(user is null)
                return Result.NotFound("User", authenticationId);

            var membership = await repository.GetByUserIdAsync(user.Id, ct);

            if (membership is null)
                return Result.NotFound("Membership", user.Id.Value);

            membership.ActivateMembership();

            await unitOfWork.SaveChangesAsync(ct);
            
            return Result.Success();
        }

        // TODO: Check if membership is active or expired exception to create a new
        public async Task<Result> CreateAsync(AuthenticationId authenticationId, CreateMembershipDTO dto, CancellationToken ct = default)
        {
            var user = await userRepository.GetByAuthenticationIdAsync(authenticationId, ct);

            if(user is null)
                return Result.NotFound("User", authenticationId);

            var membershipType = await membershipTypeRepository.GetByIdAsync(new MembershipTypeId(dto.MembershipTypeId), ct);

            if (membershipType is null)
                return Result.NotFound("MembershipType", dto.MembershipTypeId);

            var existing = await repository.GetByUserIdAsync(user.Id, ct);

            if (existing is not null && existing.IsActive)
                return Result.Conflict("User already has an active membership");

            var startDate = DateOnly.FromDateTime(DateTime.UtcNow);

            var endDate = startDate.AddDays(membershipType.Duration.Value);

            var membership = Membership.Create(
                user.Id,
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

        public async Task<Result> DeactivateAsync(AuthenticationId authenticationId, CancellationToken ct = default)
        {
            var user = await userRepository.GetByAuthenticationIdAsync(authenticationId, ct);

            if(user is null)
                return Result.NotFound("user", authenticationId);

            var membership = await repository.GetByUserIdAsync(user.Id, ct);

            if (membership is null)
                return Result.NotFound("Membership", user.Id.Value);

            await CancelAllBookings(user.Id, ct);

            membership.DeactivateMembership();

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }

        public async Task<Result<MembershipDTO>> GetByUserIdAsync(AuthenticationId authenticationId, CancellationToken ct = default)
        {
            var user = await userRepository.GetByAuthenticationIdAsync(authenticationId, ct);

            if(user is null)
                return Result<MembershipDTO>.NotFound("User", authenticationId);

            var membership = await repository.GetByUserIdAsync(user.Id, ct);

            if(membership is null)
                return Result<MembershipDTO>.NotFound("Membership", user.Id.Value);

            var membershipType = await membershipTypeRepository.GetByIdAsync(membership.TypeId, ct);

            return Result<MembershipDTO>.Success(membership.ToDTO(
                membershipType?.Name.Value ?? "",
                membershipType?.Price.Value ?? 0
            ));
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

        public async Task<Result> DeleteAsync(AuthenticationId authenticationId, CancellationToken ct = default)
        {
            var user = await userRepository.GetByAuthenticationIdAsync(authenticationId, ct);

            if(user is null)
                return Result.NotFound("User", authenticationId);

            var membership = await repository.GetByUserIdAsync(user.Id, ct);

            if(membership is null)
                return Result.NotFound("Membership", user.Id.Value);

            await CancelAllBookings(user.Id, ct);

            await repository.DeleteAsync(membership.Id, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task CancelAllBookings(UserId userId, CancellationToken ct = default)
        {
            var bookedSessions = await trainingSessionRepository.GetByUserBookingsAsync(userId, ct);

            foreach(var session in bookedSessions)
                session.CancelBooking(userId);
        }
    }
}
