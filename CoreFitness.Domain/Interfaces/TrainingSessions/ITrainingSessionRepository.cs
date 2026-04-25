using CoreFitness.Domain.Entities.TrainingSessions;
using CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;

namespace CoreFitness.Domain.Interfaces.TrainingSessions;

public interface ITrainingSessionRepository : IBaseRepository<TrainingSession, TrainingSessionId>
{
    Task<IEnumerable<TrainingSession>> GetUpcomingAsync(CancellationToken ct = default);
}