using CoreFitness.Domain.Entities.TrainingSessions;
using CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;
using CoreFitness.Domain.Entities.Users.ValueObjects;

namespace CoreFitness.Domain.Interfaces.TrainingSessions
{
    public interface ITrainingSessionRepository : IBaseRepository<TrainingSession, TrainingSessionId>
    {
        Task<IEnumerable<TrainingSession>> GetUpcomingAsync(CancellationToken ct = default);
        Task<IEnumerable<TrainingSession>> GetByUserBookingsAsync(UserId userId, CancellationToken ct = default);
    }
}
