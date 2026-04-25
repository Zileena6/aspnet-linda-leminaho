using CoreFitness.Domain.Exceptions;

namespace CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;

public readonly record struct TrainingSessionId
{
    public Guid Value { get; }

    public TrainingSessionId(Guid value)
    {
        if (value == Guid.Empty)
            throw new IdIsRequiredException();

        Value = value;
    }

    public static TrainingSessionId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}