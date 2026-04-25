using CoreFitness.Domain.Exceptions;

namespace CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;

public sealed record TrainingSessionCapacity
{
    public int Value { get; }

    private TrainingSessionCapacity(int value)
    {
        Value = value;
    }

    public static TrainingSessionCapacity Create(int value)
    {
        if (value <= 0)
            throw new InvalidCapacityException(value);

        return new TrainingSessionCapacity(value);
    }
}