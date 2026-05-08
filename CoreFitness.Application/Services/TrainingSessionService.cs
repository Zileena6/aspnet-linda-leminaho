using CoreFitness.Application.DTOs.Booking;
using CoreFitness.Application.DTOs.TrainingSession;
using CoreFitness.Application.Interfaces;
using CoreFitness.Application.Mappings;
using CoreFitness.Domain.Common;
using CoreFitness.Domain.Entities.TrainingSessions;
using CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;
using CoreFitness.Domain.Entities.Users.ValueObjects;
using CoreFitness.Domain.Interfaces.Memberships;
using CoreFitness.Domain.Interfaces.TrainingSessions;
using CoreFitness.Domain.Interfaces.UnitOfWork;
using CoreFitness.Domain.Interfaces.Users;

namespace CoreFitness.Application.Services
{
    public class TrainingSessionService(ITrainingSessionRepository repository, IUserRepository userRepository, IMembershipRepository membershipRepository, IUnitOfWork unitOfWork) : ITrainingSessionService
    {
        public async Task<Result<TrainingSessionDTO>> GetByIdAsync(Guid sessionId, CancellationToken ct = default)
        {
            var session = await repository.GetByIdAsync(new TrainingSessionId(sessionId), ct);
            
            if (session is null)
                return Result<TrainingSessionDTO>.NotFound("TrainingSession", sessionId);

            return Result<TrainingSessionDTO>.Success(session.ToDTO());
        }

        public async Task<Result> BookAsync(Guid sessionId, AuthenticationId authId, CancellationToken ct = default)
        {
            var session = await repository.GetByIdAsync(new TrainingSessionId(sessionId), ct);

            if (session is null)
                return Result.NotFound("TrainingSession", sessionId);            

            var user = await userRepository.GetByAuthenticationIdAsync(authId, ct);

            if(user is null)
                return Result.NotFound("User", authId);

            var uId = user.Id;

            var membership = await membershipRepository.GetByUserIdAsync(uId, ct);

            if (membership is null || !membership.IsActive)
                return Result.Conflict("User does not have an active membership");

            if(!membership.HasSessionsLeft)
                return Result.Conflict("So sessions left");

            var bookingResult = session.Book(uId);

            if(bookingResult.IsFailure)
                return Result.Failure(bookingResult.Error!);

            membership.UseSession();

            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        // TODO: Return session to member?
        public async Task<Result> CancelBookingAsync(Guid sessionId, AuthenticationId authId, CancellationToken ct = default)
        {
            var session = await repository.GetByIdAsync(new TrainingSessionId(sessionId), ct);

            if (session is null)
                return Result.NotFound("TrainingSession", sessionId);

            var user = await userRepository.GetByAuthenticationIdAsync(authId, ct);

            if(user is null)
                return Result.NotFound("User", authId);

            var uId = user.Id;

            var membership = await membershipRepository.GetByUserIdAsync(uId, ct);

            var cancelResult = session.CancelBooking(uId);

            if(cancelResult.IsFailure)
                return cancelResult;

            if (membership is not null && membership.IsActive)
                membership.RefundSession();

            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task<Result<TrainingSessionDTO>> CreateAsync(CreateTrainingSessionDTO dto, CancellationToken ct = default)
        {
            var session = TrainingSession.Create(
                TrainingSessionName.Create(dto.Name),
                TrainingSessionDescription.Create(dto.Description),
                dto.StartDate,
                TrainingSessionCapacity.Create(dto.Capacity),
                TrainingSessionDuration.FromMinutes(dto.DurationInMinutes));

            await repository.AddAsync(session, ct);
            
            await unitOfWork.SaveChangesAsync(ct);

            return Result<TrainingSessionDTO>.Success(session.ToDTO());

        }

        public async Task<Result> DeleteAsync(Guid sessionId, CancellationToken ct = default)
        {
            var deleted = await repository.DeleteAsync(new TrainingSessionId(sessionId), ct);
            if (!deleted)
                return Result.NotFound("TrainingSession", sessionId);

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }

        public async Task<Result<IEnumerable<TrainingSessionDTO>>> GetUpcomingAsync(CancellationToken ct = default)
        {
            var sessions = await repository.GetUpcomingAsync(ct);
            
            return Result<IEnumerable<TrainingSessionDTO>>.Success(sessions.Select(s => s.ToDTO()));
        }

        public async Task<Result<IEnumerable<BookingDTO>>> GetUserBookingsAsync(Guid userId, CancellationToken ct = default)
        {
            var sessions = await repository.GetUpcomingAsync(ct);

                foreach(var s in sessions)
        foreach(var b in s.Bookings)
            Console.WriteLine($"Booking UserId: {b.UserId}");

    Console.WriteLine($"Looking for userId: {userId}");

            var bookings = sessions
                .SelectMany(s => s.Bookings
                .Where(b => b.UserId == new UserId(userId))
                .Select(b => b.ToDTO(s)));

            return Result<IEnumerable<BookingDTO>>.Success(bookings);
        }

        public async Task<Result> UpdateAsync(UpdateTrainingSessionDTO dto, CancellationToken ct = default)
        {

            var session = await repository.GetByIdAsync(new TrainingSessionId(dto.Id), ct);
            if (session is null)
                return Result.NotFound("TrainingSession", dto.Id);

            session.UpdateAll(
                TrainingSessionName.Create(dto.Name),
                TrainingSessionDescription.Create(dto.Description),
                dto.StartDate,
                TrainingSessionDuration.FromMinutes(dto.DurationInMinutes),
                TrainingSessionCapacity.Create(dto.Capacity));

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
