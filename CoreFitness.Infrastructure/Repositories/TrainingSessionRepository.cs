using CoreFitness.Domain.Entities.TrainingSessions;
using CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;
using CoreFitness.Domain.Entities.Users.ValueObjects;
using CoreFitness.Domain.Interfaces.TrainingSessions;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Repositories
{
    public class TrainingSessionRepository(CoreFitnessDbContext context) : BaseRepository<TrainingSession, TrainingSessionId>(context),
        ITrainingSessionRepository
    {
        public override async Task<TrainingSession?> GetByIdAsync(TrainingSessionId id, CancellationToken ct = default)
        {
            return await _context.TrainingSessions
                .Include(ts => ts.Bookings)
                .FirstOrDefaultAsync(ts => ts.Id == id, ct);
        }

        public async Task<IEnumerable<TrainingSession>> GetByUserBookingsAsync(UserId userId, CancellationToken ct = default)
        {
            return await _context.TrainingSessions
                .Include(s => s.Bookings)
                .Where(s => s.Bookings.Any(b => b.UserId == userId))
                .ToListAsync();
        }

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
}
