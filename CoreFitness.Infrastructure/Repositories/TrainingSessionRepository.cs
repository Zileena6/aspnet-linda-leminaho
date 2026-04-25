using CoreFitness.Domain.Entities.TrainingSessions;
using CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;
using CoreFitness.Domain.Interfaces.TrainingSessions;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Repositories;

public class TrainingSessionRepository(CoreFitnessDbContext context) : BaseRepository<TrainingSession, TrainingSessionId>(context),
        ITrainingSessionRepository
{
    public async Task<IEnumerable<TrainingSession>> GetUpcomingAsync(CancellationToken ct = default) =>
        await _context.TrainingSessions
        .Include(ts => ts.Bookings)
        .Where(ts => ts.StartDate > DateTimeOffset.UtcNow)
        .OrderBy(ts => ts.StartDate)
        .ToListAsync(ct);
}