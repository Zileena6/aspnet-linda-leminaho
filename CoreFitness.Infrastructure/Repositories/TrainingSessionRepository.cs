using CoreFitness.Domain.Entities.TrainingSessions;
using CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;
using CoreFitness.Domain.Interfaces.TrainingSessions;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Repositories;

public class TrainingSessionRepository(CoreFitnessDbContext context) : BaseRepository<TrainingSession, TrainingSessionId>(context),
        ITrainingSessionRepository
{
    public async Task<IEnumerable<TrainingSession>> GetUpcomingAsync(CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;

        return await _context.TrainingSessions
        .Include(ts => ts.Bookings)
        .Where(ts => ts.StartDate > now)
        .OrderBy(ts => ts.StartDate)
        .ToListAsync(ct);
    }
}