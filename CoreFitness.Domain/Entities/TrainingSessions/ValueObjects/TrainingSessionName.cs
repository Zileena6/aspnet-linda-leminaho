using CoreFitness.Domain.Exceptions;

namespace CoreFitness.Domain.Entities.TrainingSessions.ValueObjects;

public readonly partial record struct TrainingSessionName
{
    public const int MaxLength = 100;

    public string Value { get; }

    private TrainingSessionName(string value)
    {
        Value = value;
    }

    public static TrainingSessionName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidTrainingSessionNameException("Name is required");

        var trimmed = value.Trim();

        if (trimmed.Length > MaxLength)
            throw new InvalidTrainingSessionNameException($"Name cannot contain more than {MaxLength} characters");

        return new TrainingSessionName(trimmed);
    }

    public override string ToString() => Value;
}