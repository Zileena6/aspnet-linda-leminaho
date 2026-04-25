using CoreFitness.Domain.Exceptions;

namespace CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;

public readonly record struct TrainingSessionDuration
{
    public TimeSpan Value { get; }

    private TrainingSessionDuration(TimeSpan value)
    {
        Value = value;
    }

    public static TrainingSessionDuration Create(TimeSpan value)
    {
        if (value <= TimeSpan.Zero)
            throw new InvalidDurationException(value);

        return new TrainingSessionDuration(value);
    }

    public static TrainingSessionDuration FromMinutes(int minutes) =>
        Create(TimeSpan.FromMinutes(minutes));

    public int TotalMinutes => (int)Value.TotalMinutes;
}