using CoreFitness.Application.DTOs.Booking;
using CoreFitness.Application.DTOs.TrainingSession;
using CoreFitness.Domain.Common;
using CoreFitness.Domain.Entities.Users.ValueObjects;

namespace CoreFitness.Application.Interfaces
{
    public interface ITrainingSessionService
    {
        Task<Result<IEnumerable<TrainingSessionDTO>>> GetUpcomingAsync(CancellationToken ct = default);
        Task<Result<TrainingSessionDTO>> GetByIdAsync(Guid sessionId, CancellationToken ct = default);
        Task<Result<IEnumerable<BookingDTO>>> GetUserBookingsAsync(Guid userId, CancellationToken ct = default);
        Task<Result<TrainingSessionDTO>> CreateAsync(CreateTrainingSessionDTO dto, CancellationToken ct = default);
        Task<Result> UpdateAsync(UpdateTrainingSessionDTO dto, CancellationToken ct = default);
        Task<Result> DeleteAsync(Guid sessionId, CancellationToken ct = default);
        Task<Result> BookAsync(Guid sessionId, AuthenticationId authId, CancellationToken ct = default);
        Task<Result> CancelBookingAsync(Guid sessionId, AuthenticationId authId, CancellationToken ct = default);
    }
}
